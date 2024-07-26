using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TitleSceneManager : MonoBehaviour
{
    // ��ư
    public Button startBtn;
    public Button exitBtn;
    private Button[] btns;
    private int selectIndex = 0;

    // ����
    Color normalColor = Color.white;
    Color selectedColor = Color.yellow;

    // ĳ���� �ִϸ��̼�
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
        // Ű����� ��ư ����
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            selectIndex = (selectIndex - 1 + btns.Length) % btns.Length;                    // ��Ű Ŭ���� �ε��� ����
            SlectButtonColor();                                                             // ���� ����
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            selectIndex = (selectIndex + 1) % btns.Length;                                  // �Ʒ�Ű Ŭ���� �ε��� ����
            SlectButtonColor();                                                             // ���� ����
        }

        if (Input.GetKeyDown(KeyCode.S))                                                    // SŰ Ŭ���� ���� Ȯ��
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

    void SlectButtonColor()                            // ���� ���� �� ��ư ���� ����
    {
        for (int i = 0; i < btns.Length; i++)          // �ε��� ��ȣ�� ��ġ�ϸ� ����� �ƴϸ� ���
        {
            btns[i].GetComponent<Image>().color = (i == selectIndex) ? selectedColor : normalColor;
        }
    }

    // ���� �ϸ�
    void ExecuteSelect()
    {
        isSelet = true;                         // ĳ���� �ִϸ��̼��� ���� bool ����
        switch (selectIndex)                    // �ε��� ��ȣ�� ��ġ�ϴ� case�� �Լ� ����
        {
            case 0:                             
                StartCoroutine(StartAnim());    // ���۾����� ����
                break;
            case 1:
                StartCoroutine(ExitAnim());     // ���� ����
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
