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
        // 시작할 때 플레이어를 찾음
        player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            playerMove = player.GetComponent<PlayerMove>();
        }
    }

    // 사다리와 접촉하면 방향키(위,아래) 누를때, 오르고 내릴 수 있게 해줌)
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            isLadder = true;
            UpdateLadderState();
        }
    }

    // 지형 위에서 사다리 타고 내려올때 필요한거
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Physics2D.IgnoreCollision( collision.GetComponent<Collider2D>(), ladderGround, true );
            collision.GetComponent<Rigidbody2D>().gravityScale = 0f;
        }
    }

    // 사다리와 접촉이 끝나면
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            // 사다리 안타고 있다고 PlayerMove에 전달함
            isLadder = false;
            // 지형 무시하고 내려가지 못하게
            Physics2D.IgnoreCollision(collision.GetComponent<Collider2D>(), ladderGround, false);
            collision.GetComponent<Rigidbody2D>().gravityScale = 1f;
            UpdateLadderState();
        }
    }

    //PlayerMove에 값을 전달하는 함수
    private void UpdateLadderState()
    {
        if (playerMove != null)
        {
            playerMove.SetLadderState(isLadder);
        }
    }
}
