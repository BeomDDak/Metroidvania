using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shop : MonoBehaviour
{
    private GameObject _player;
    private PlayerMove _playerInterect;
    [SerializeField] private GameObject shopUI;

    private void Start()
    {
        _player = GameObject.FindGameObjectWithTag("Player");
        _playerInterect = _player.GetComponent<PlayerMove>();
    }

    private void Update()
    {
        if (_playerInterect.pressInteract)
        {
            shopUI.SetActive(true);
        }
        _playerInterect.pressInteract = false;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            shopUI.SetActive(false);
        }
    }
}
