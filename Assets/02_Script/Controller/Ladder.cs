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
    private void Update()
    {
        if (player != null)
        {
            playerMove = player.GetComponent<PlayerMove>();
        }
    }

    // Ʈ���Ź����� �Ͼ�� isLadder ���� PlayerMove�� ��ȯ��
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            isLadder = true;
            UpdateLadderState();
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Physics2D.IgnoreCollision( collision.GetComponent<Collider2D>(), ladderGround, true );
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            isLadder = false;
            Physics2D.IgnoreCollision(collision.GetComponent<Collider2D>(), ladderGround, false);

            UpdateLadderState();
        }
    }


    private void UpdateLadderState()
    {
        if (playerMove != null)
        {
            playerMove.SetLadderState(isLadder);
        }
    }
}
