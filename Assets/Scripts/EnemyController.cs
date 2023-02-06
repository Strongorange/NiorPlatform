using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    Rigidbody2D rigid;
    Animator animator;
    SpriteRenderer spriteRenderer;
    BoxCollider2D collider;
    public int nextMove;
    public int invokeTime = 5;

    // Start is called before the first frame update
    void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        collider = GetComponent<BoxCollider2D>();
        Think();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        // 움직임
        rigid.velocity = new Vector2(nextMove, rigid.velocity.y);

        // 플랫폼 절벽 체크
        Vector2 frontVec = new Vector2(rigid.position.x + nextMove * 0.3f, rigid.position.y);
        Debug.DrawRay(frontVec, Vector2.down, new Color(1, 0, 0));
        RaycastHit2D rayHit = Physics2D.Raycast(
            frontVec,
            Vector2.down,
            1,
            LayerMask.GetMask("Platform")
        );
        if (rayHit.collider == null)
        {
            Turn();
        }
    }

    void Think()
    {
        // Range 에서 최소값은 포람 되지만 최대값은 포함하지 않는 범위가 나옴
        nextMove = Random.Range(-1, 2);

        // 애니메이션 제어
        animator.SetInteger("walkSpeed", nextMove);
        // FlipX 0이 아닐때만 방향 전환
        if (nextMove != 0)
        {
            spriteRenderer.flipX = nextMove == 1;
        }

        // Range 에서 최소값은 포람 되지만 최대값은 포함하지 않는 범위가 나옴
        float nextThinkTime = Random.Range(2f, 5f);
        Invoke("Think", nextThinkTime);
    }

    void Turn()
    {
        nextMove *= -1;
        spriteRenderer.flipX = nextMove == 1;
        CancelInvoke();
        Invoke("Think", 4);
    }

    public void OnDamaged()
    {
        // 스프라이트 투명도 (알파)
        spriteRenderer.color = new Color(1, 1, 1, 0.4f);
        // 스프라이트 플립 Y
        spriteRenderer.flipY = true;
        // Collider Disable
        collider.enabled = false;
        // 사망 이펙트
        rigid.AddForce(Vector2.up * 5, ForceMode2D.Impulse);
        // Destroy
        Invoke("Deactive", 4);
    }

    void Deactive()
    {
        gameObject.SetActive(false);
    }
}
