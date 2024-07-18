using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteract : MonoBehaviour
{

    public bool pressInteract = false;

    public GameObject go;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            print(pressInteract);
        }
        if (Input.GetKeyUp(KeyCode.Space))
        {
            print(pressInteract);
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {

        if (collision.CompareTag("Player"))
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                pressInteract = true;
            }
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
