using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TitleSceneManager : MonoBehaviour
{
    // 버튼
    public Button startBtn;
    public Button exitBtn;
    private Button[] btns;
    private int selectIndex = 0;

    // 색깔
    Color normalColor = Color.white;
    Color selectedColor = Color.yellow;

    // 캐릭터 애니메이션
    public GameObject titlePlayer;
    Animator anim;
    float speed = 3;
    bool isWalk = true;
    bool isSelet = false;

    void Start()
    {
        btns = new Button[] { startBtn, exitBtn };
        SlectButtonColor();
        anim = titlePlayer.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        // 키보드로 버튼 변경
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            selectIndex = (selectIndex - 1 + btns.Length) % btns.Length;                    // 윗키 클릭시 인덱스 감소
            SlectButtonColor();                                                             // 색깔 변경
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            selectIndex = (selectIndex + 1) % btns.Length;                                  // 아랫키 클릭시 인덱스 증가
            SlectButtonColor();                                                             // 색깔 변경
        }

        if (Input.GetKeyDown(KeyCode.S))                                                    // S키 클릭시 선택 확정
        {
            ExecuteSelect();                                                                
        }

        if (isWalk && !isSelet)
        {
            titlePlayer.transform.Translate(Vector2.right * speed * Time.deltaTime);
            if (titlePlayer.transform.position.x >= 0)
            {
                isWalk = false;
            }
        }
        else if (!isWalk && !isSelet)
        {
            anim.Play("Idle");
        }

    }

    void SlectButtonColor()                            // 선택 변경 시 버튼 색깔 변경
    {
        for (int i = 0; i < btns.Length; i++)          // 인덱스 번호와 일치하면 노랑색 아니면 흰색
        {
            btns[i].GetComponent<Image>().color = (i == selectIndex) ? selectedColor : normalColor;
        }
    }

    // 선택 하면
    void ExecuteSelect()
    {
        isSelet = true;                         // 캐릭터 애니메이션을 위한 bool 변수
        switch (selectIndex)                    // 인덱스 번호와 일치하는 case의 함수 실행
        {
            case 0:                             
                StartCoroutine(StartAnim());    // 시작씬으로 변경
                break;
            case 1:
                StartCoroutine(ExitAnim());     // 게임 종료
                break;
        }
    }

    IEnumerator StartAnim()
    {
        anim.Play("Start");
        yield return new WaitForSeconds(1.5f);
        SceneManager.LoadScene("Villige");
    }

    IEnumerator ExitAnim()
    {
        anim.Play("Exit");
        yield return new WaitForSeconds(1f);
        Application.Quit();
    }
}
