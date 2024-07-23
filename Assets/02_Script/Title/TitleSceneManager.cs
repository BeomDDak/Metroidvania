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
            selectIndex = (selectIndex - 1 + btns.Length) % btns.Length;
            SlectButtonColor();
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            selectIndex = (selectIndex + 1) % btns.Length;
            SlectButtonColor();
        }

        if (Input.GetKeyDown(KeyCode.S))
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

    // 선택 변경 시 버튼 색깔 변경
    void SlectButtonColor()
    {
        for (int i = 0; i < btns.Length; i++)
        {
            btns[i].GetComponent<Image>().color = (i == selectIndex) ? selectedColor : normalColor;
        }
    }

    // 선택 하면
    void ExecuteSelect()
    {
        isSelet = true;
        switch (selectIndex)
        {
            case 0:
                StartCoroutine(StartAnim());
                break;
            case 1:
                StartCoroutine(ExitAnim());
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
