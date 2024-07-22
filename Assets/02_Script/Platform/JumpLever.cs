using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpLever : MonoBehaviour
{
    PlayerMove _interact;
    bool isTrigging = false;

    public GameObject leverOn;
    public GameObject toGoHome;

    void Start()
    {
        _interact = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMove>();
    }

    // Update is called once per frame
    void Update()
    {
        if (_interact.pressInteract && isTrigging)
        {
            this.gameObject.SetActive(false);
            leverOn.SetActive(true);
            toGoHome.SetActive(true);
            DataManager.instance.stage1Clear = true;

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
