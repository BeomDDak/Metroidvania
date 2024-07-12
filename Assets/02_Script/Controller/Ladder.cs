using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Ladder : MonoBehaviour
{
    public bool isLadder = false;
    PlayerMove playerMove;
    GameObject player;
    public Collider2D ladderGround;

    private void Start()
    {
        // ������ �� �÷��̾ ã��
        player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            playerMove = player.GetComponent<PlayerMove>();
        }
    }

    // ��ٸ��� �����ϸ� ����Ű(��,�Ʒ�) ������, ������ ���� �� �ְ� ����)
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            isLadder = true;
            UpdateLadderState();
        }
    }

    // ���� ������ ��ٸ� Ÿ�� �����ö� �ʿ��Ѱ�
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Physics2D.IgnoreCollision( collision.GetComponent<Collider2D>(), ladderGround, true );
            collision.GetComponent<Rigidbody2D>().gravityScale = 0f;
        }
    }

    // ��ٸ��� ������ ������
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            // ��ٸ� ��Ÿ�� �ִٰ� PlayerMove�� ������
            isLadder = false;
            // ���� �����ϰ� �������� ���ϰ�
            Physics2D.IgnoreCollision(collision.GetComponent<Collider2D>(), ladderGround, false);
            collision.GetComponent<Rigidbody2D>().gravityScale = 1f;
            UpdateLadderState();
        }
    }

    //PlayerMove�� ���� �����ϴ� �Լ�
    private void UpdateLadderState()
    {
        if (playerMove != null)
        {
            playerMove.SetLadderState(isLadder);
        }
    }
}
