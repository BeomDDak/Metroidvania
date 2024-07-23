using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreasureBox : MonoBehaviour
{
    PlayerMove _interact;
    bool isTrigging = false;
    public GameObject openBox;
    public GameObject jam;

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
            openBox.SetActive(true);
            Instantiate(jam, transform.position, Quaternion.identity);
            DataManager.instance.jam += 500;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            isTrigging = true;
        }
    }
}
