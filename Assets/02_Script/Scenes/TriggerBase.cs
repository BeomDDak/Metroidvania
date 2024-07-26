using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerBase : MonoBehaviour
{
    protected bool canInteract = false;             // 콜라이더에 닿았는지 확인할 bool 변수
    GameObject player;  
    PlayerMove _playerMove;

    // 플레이어 찾기
    /*public virtual void ResetReferences()           // 씬 이동해도 계속 찾기 위한 함수
    {
        player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            _playerMove = player.GetComponent<PlayerMove>();
        }
    }*/

    void Start()
    {
        //ResetReferences();
        player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            _playerMove = player.GetComponent<PlayerMove>();
        }
    }

    
    // 실행문
    void Update()
    {
        if (canInteract && !SceneSwapManager.isTransitioning)
        {
            if (_playerMove.pressInteract)
            {
                Debug.Log($"Interact triggered on {gameObject.name}");
                Interact();
            }
        }
    }

    //  ===============트리거==================
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Debug.Log($"Player entered trigger area of {gameObject.name}");
            canInteract = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Debug.Log($"Player exited trigger area of {gameObject.name}");
            canInteract = false;
        }
    }

    // =============== 콜라이션 ==================
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Debug.Log($"Player entered trigger area of {gameObject.name}");
            canInteract = true;
            _playerMove.pressInteract = true;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Debug.Log($"Player entered trigger area of {gameObject.name}");
            canInteract = false;
            _playerMove.pressInteract = false;
        }
    }

    // 자식 클래스에서 재정의
    public virtual void Interact() { }

}
