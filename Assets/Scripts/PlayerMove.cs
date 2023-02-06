using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    public GameManager gameManager;
    public float maxSpeed = 20;
    public float jumpPower = 10;
    Rigidbody2D rigid;
    SpriteRenderer spriteRenderer;
    Animator animator;

    BoxCollider2D boxCollider;

    private const int playerLayer = 10;
    private const int plyaerDamagedLayer = 11;

    // Start is called before the first frame update
    void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        boxCollider = GetComponent<BoxCollider2D>();
    }

    // Update is called once per frame
    // 단발적인 일반적인 키 입력
    void Update()
    {
        //점프
        if (Input.GetButtonDown("Jump") && !animator.GetBool("isJumping"))
        {
            rigid.AddForce(Vector2.up * jumpPower, ForceMode2D.Impulse);
            animator.SetBool("isJumping", true);
        }

        // 손 땟을때 자동으로 멈추게
        if (Input.GetButtonUp("Horizontal"))
        {
            rigid.velocity = new Vector2(rigid.velocity.normalized.x * 0.5f, rigid.velocity.y);
        }

        // 방향 전환
        if (Input.GetButton("Horizontal"))
        {
            spriteRenderer.flipX = Input.GetAxisRaw("Horizontal") == -1;
        }

        // 애니메이션
        if (Mathf.Abs(rigid.velocity.x) < 0.3)
        {
            // 멈춘 상태
            animator.SetBool("isWalking", false);
        }
        else
        {
            // 움직이고 있는 상태
            animator.SetBool("isWalking", true);
        }
    }

    // FixedUpdate 는 물리계산같은게 들어갈때
    void FixedUpdate()
    {
        // 오른쪽 왼쪽 움직임 제어
        float h = Input.GetAxisRaw("Horizontal");
        rigid.AddForce(Vector2.right * h, ForceMode2D.Impulse);
        if (rigid.velocity.x > maxSpeed)
        {
            // 오른쪽 최대속력
            rigid.velocity = new Vector2(maxSpeed, rigid.velocity.y);
        }
        else if (rigid.velocity.x < maxSpeed * (-1))
        {
            //왼쪽 최대속력
            rigid.velocity = new Vector2(maxSpeed * (-1), rigid.velocity.y);
        }

        // Landing Platform
        // 하강시에만 Ray 발사
        if (rigid.velocity.y < 0)
        {
            Debug.DrawRay(rigid.position, Vector2.down, new Color(1, 0, 0));
            RaycastHit2D rayHit = Physics2D.Raycast(
                rigid.position,
                Vector2.down,
                1,
                LayerMask.GetMask("Platform")
            );
            if (rayHit.collider != null)
            {
                // Ray 는 Player 가운데서 나옴 , 플레이어 크기는 1
                if (rayHit.distance < 0.5f)
                {
                    animator.SetBool("isJumping", false);
                }
            }
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            // 공격
            if (rigid.velocity.y < 0 && transform.position.y > collision.transform.position.y)
            {
                OnAttack(collision.transform);
            }
            else
            {
                OnDamaged(collision.transform.position);
            }
        }
    }

    void OnDamaged(Vector2 targetPosition)
    {
        // 체력 깎임
        gameManager.HealthDown();
        // 무적
        gameObject.layer = plyaerDamagedLayer;
        spriteRenderer.color = new Color(1, 1, 1, 0.4f);

        // 반작용 튕겨나감
        int direction = transform.position.x - targetPosition.x > 0 ? 1 : -1;
        rigid.AddForce(new Vector2(direction, 1) * 13, ForceMode2D.Impulse);

        // 애니메이션
        animator.SetTrigger("Damaged");

        Invoke("OffDamaged", 3);
    }

    void OnAttack(Transform enemy)
    {
        // 점수
        gameManager.stagePoint += 100;
        // 반발력
        rigid.AddForce(Vector2.up * 10, ForceMode2D.Impulse);

        // 적 물리침
        EnemyController enemyController = enemy.GetComponent<EnemyController>();
        enemyController.OnDamaged();
    }

    void OffDamaged()
    {
        // 레이어 바꿔서 무적 표현
        gameObject.layer = playerLayer;
        spriteRenderer.color = new Color(1, 1, 1, 1);
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Item")
        {
            bool isBronze = collision.gameObject.name.Contains("Bronze");
            bool isSilver = collision.gameObject.name.Contains("Silver");
            bool isGold = collision.gameObject.name.Contains("Gold");
            // Point
            if (isBronze)
            {
                gameManager.stagePoint += 50;
            }
            else if (isSilver)
            {
                gameManager.stagePoint += 100;
            }
            else if (isGold)
            {
                gameManager.stagePoint += 300;
            }

            // Destory
            collision.gameObject.SetActive(false);
        }
        else if (collision.gameObject.tag == "Finish")
        {
            // 스테이지 이동
            gameManager.NextStage();
        }
    }

    public void OnDie()
    {
        // 스프라이트 투명도 (알파)
        spriteRenderer.color = new Color(1, 1, 1, 0.4f);
        // 스프라이트 플립 Y
        spriteRenderer.flipY = true;
        // Collider Disable
        boxCollider.enabled = false;
        // 사망 이펙트
        rigid.AddForce(Vector2.up * 5, ForceMode2D.Impulse);

        Debug.Log("게임 끝!");
    }

    public void VelocityZero()
    {
        rigid.velocity = Vector2.zero;
    }
}
