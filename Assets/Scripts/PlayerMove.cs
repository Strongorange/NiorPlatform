using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    public float maxSpeed;
    Rigidbody2D rigid;
    SpriteRenderer spriteRenderer;
    Animator animator;
    // Start is called before the first frame update
    void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    // 일반적인 키 입력
    void Update() {
        // 손 땟을때 자동으로 멈추게
        if(Input.GetButtonUp("Horizontal")) {
            rigid.velocity = new Vector2(rigid.velocity.normalized.x * 0.5f, rigid.velocity.y);
        }

        // 방향 전환
        if(Input.GetButtonDown("Horizontal")) {
            spriteRenderer.flipX = Input.GetAxisRaw("Horizontal") == -1;
        }

        if(Mathf.Abs(rigid.velocity.x) < 0.3) {
            // 멈춘 상태
            animator.SetBool("isWalking", false);
        } else {
            // 움직이고 있는 상태
            animator.SetBool("isWalking", true);
        }

        
    }

    // FixedUpdate 는 물리계산같은게 들어갈때
    void FixedUpdate(){
        // 오른쪽 왼쪽 움직임 제어
        float h = Input.GetAxisRaw("Horizontal");
        rigid.AddForce(Vector2.right * h, ForceMode2D.Impulse);
        if(rigid.velocity.x > maxSpeed) { 
            // 오른쪽 최대속력
            rigid.velocity = new Vector2(maxSpeed, rigid.velocity.y);
        } else if (rigid.velocity.x < maxSpeed * (-1)) {
            //왼쪽 최대속력
            rigid.velocity = new Vector2(maxSpeed * (-1), rigid.velocity.y);
        }
    }
}
