using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class GameManager : MonoBehaviour
{
    public int totalPoint;
    public int stagePoint;
    public int stageIndex;
    public int health;

    public PlayerMove player;

    public CameraConfiner cameraConfiner;
    public GameObject[] Stages;
    public Image[] UIhealth;
    public TextMeshProUGUI UIPoint;
    public TextMeshProUGUI UIStage;
    public GameObject UIRestartBtn;

    BoxCollider2D boxCollider;

    // Start is called before the first frame update
    void Awake()
    {
        health = 5;
        stageIndex = 0;
        boxCollider = GetComponent<BoxCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        UIPoint.text = (stagePoint + totalPoint).ToString();
    }

    public void NextStage()
    {
        // 스테이지 변경
        if (stageIndex < Stages.Length - 1)
        {
            Stages[stageIndex].SetActive(false);
            stageIndex++;
            Stages[stageIndex].SetActive(true);
            PlayerReposition();
            Debug.Log("다음 스테이지로");
            UIStage.text = "STAGE " + (stageIndex + 1);
            RepositionGameManager(stageIndex);
        }
        else
        {
            // 게임 클리어
            Time.timeScale = 0;
            Debug.Log("게임 클리어");

            // 재식작 버튼 UI
            TMP_Text btnText = UIRestartBtn.GetComponentInChildren<TMP_Text>();
            if (btnText != null)
            {
                Debug.Log(btnText);
                btnText.text = "Game Clear!";
                ViewBtn();
            }
        }

        cameraConfiner.SwitchConfiner(stageIndex);
        HealthUp();

        // 점수 계산
        totalPoint += stagePoint;
        stagePoint = 0;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            // 플레이어 땅으로 복구, Player Reposition
            if (health > 1)
            {
                PlayerReposition();
            }
            // 체력 하락
            HealthDown();
        }
    }

    public void HealthUp()
    {
        if (health >= 5)
        {
            return;
        }
        else
        {
            health++;
            UIhealth[health - 1].color = new Color(1, 1, 1);
        }
    }

    public void HealthDown()
    {
        if (health > 1)
        {
            health--;
            UIhealth[health].color = new Color(1, 1, 1, 0.2f);
        }
        else
        {
            // 체력 UI OFF
            UIhealth[0].color = new Color(1, 1, 1, 0.2f);
            // 플레이어 사망 이펙트
            player.OnDie();
            // 결과 UI
            Debug.Log("게임 종료");
            // 재시도 버튼 UI
            ViewBtn();
        }
    }

    void PlayerReposition()
    {
        player.transform.position = new Vector3(0, 0, -1);
        player.VelocityZero();
    }

    public void Restart()
    {
        Time.timeScale = 1;
        RepositionGameManager(0);
        SceneManager.LoadScene(0);
        RepositionGameManager(0);
    }

    void ViewBtn()
    {
        UIRestartBtn.SetActive(true);
    }

    public void PlayerHeal()
    {
        if (health >= 5)
        {
            return;
        }
        else
        {
            health++;
        }
    }

    void RepositionGameManager(int stageIdx)
    {
        float newOffset = 0;
        switch (stageIdx)
        {
            case 0:
                newOffset = 0;
                break;
            case 1:
                newOffset = 0;
                break;
            case 2:
                newOffset = -2;
                break;
            case 3:
                newOffset = -32;
                break;
        }

        Vector2 newOffsetVector = new Vector2(boxCollider.offset.x, newOffset);
        boxCollider.offset = newOffsetVector;
    }
}
