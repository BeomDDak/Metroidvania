using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum DoorNumber
{
    None,
    one,
    two, 
    three, 
    four, 
    five,
}

public class DoorExit : MonoBehaviour
{
    public string sceneName = "";
    public int doorNum = 0;
    public DoorNumber doorNumber = DoorNumber.None;

    GameObject player;
    PlayerMove _playerMove;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        _playerMove = player.GetComponent<PlayerMove>();
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (_playerMove.pressInteract)
            {
                SceneChange.instance.ChangeScene(sceneName,doorNum);
            }
        }
    }

}
