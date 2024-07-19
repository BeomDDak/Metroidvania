using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerBase : MonoBehaviour
{
    protected bool canInteract = false;
    GameObject player;
    PlayerMove _playerMove;

    // �÷��̾� ã��
    public virtual void ResetReferences()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            _playerMove = player.GetComponent<PlayerMove>();
        }
    }

    void Start()
    {
        ResetReferences();
    }

    
    // ���๮
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

    //  ===============Ʈ����==================
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

    // =============== �ݶ��̼� ==================
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

    // �ڽ� Ŭ�������� ������
    public virtual void Interact() { }

}
