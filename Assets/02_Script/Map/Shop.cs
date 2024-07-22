using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shop : MonoBehaviour
{
    private GameObject _player;
    private PlayerMove _playerInterect;

    private bool openShop = false;
    [SerializeField] private GameObject shopUI;

    private void Start()
    {
        _player = GameObject.FindGameObjectWithTag("Player");
        _playerInterect = _player.GetComponent<PlayerMove>();
    }

    private void Update()
    {
        if (_playerInterect.pressInteract && openShop)
        {
            shopUI.SetActive(true);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            openShop = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            shopUI.SetActive(false);
            openShop = false;
        }
    }
}
