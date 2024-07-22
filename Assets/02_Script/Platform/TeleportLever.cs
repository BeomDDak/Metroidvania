using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleportLever : MonoBehaviour
{
    PlayerMove _interact;
    bool isTrigging = false;

    public GameObject leverOn;
    public GameObject toGoHome;

    private void OnEnable()
    {
        _interact = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMove>();
    }

    void Update()
    {
        if (_interact.pressInteract && isTrigging)
        {
            this.gameObject.SetActive(false);
            leverOn.SetActive(true);
            toGoHome.SetActive(true);
            DataManager.instance.stage3Clear = true;

        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            isTrigging = true;
        }
    }
}
