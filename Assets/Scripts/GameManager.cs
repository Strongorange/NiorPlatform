using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public int totalPoint;
    public int stagePoint;
    public int stageIndex;
    public int health;

    public PlayerMove player;

    // Start is called before the first frame update
    void Awake()
    {
        health = 5;
    }

    // Update is called once per frame
    void Update() { }

    public void NextStage()
    {
        stageIndex++;
        totalPoint += stagePoint;
        stagePoint = 0;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            // 플레이어 땅으로 복구
            if (health > 1)
            {
                collision.attachedRigidbody.velocity = Vector2.zero;
                collision.transform.position = new Vector3(0, 0, -1);
            }
            // 체력 하락
            HealthDown();
        }
    }

    public void HealthDown()
    {
        if (health > 1)
        {
            health--;
        }
        else
        {
            // 플레이어 사망 이펙트
            player.OnDie();
            // 결과 UI
            Debug.Log("게임 종료");
            // 재시도 버튼 UI
        }
    }
}
