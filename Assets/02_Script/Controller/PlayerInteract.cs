using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteract : MonoBehaviour
{

    public bool pressInteract = false;

    private void OnTriggerStay2D(Collider2D collision)
    {

        if (collision.CompareTag("Interact"))
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                pressInteract = true;
                print(pressInteract);
            }
            print(pressInteract);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Interact"))
        {
            pressInteract = false;
            print(pressInteract);
        }
    }





}
