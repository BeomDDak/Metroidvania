using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerBase : MonoBehaviour
{
    protected bool canInteract = false;
    GameObject player;
    PlayerInteract _playerInteract;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        if(player != null)
        {
            _playerInteract = player.GetComponent<PlayerInteract>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (canInteract)
        {
            if (_playerInteract.pressInteract)
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
