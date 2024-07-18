using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerBase : MonoBehaviour
{
    protected bool canInteract = false;
    GameObject player;
    PlayerMove _playerMove;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        if(player != null)
        {
            _playerMove = player.GetComponent<PlayerMove>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (canInteract)
        {
            if (_playerMove.pressInteract)
            {
                Interact();
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            canInteract = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            canInteract = false;
        }
    }

    public virtual void Interact() { }

}
