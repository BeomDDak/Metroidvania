using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Teleport : MonoBehaviour
{
    public Transform toTeleport;
    //public Transform targetTeleport;
    GameObject player;
    Vector2 _telePos;

    void Start()
    {
        player = GameObject.FindWithTag("Player");
    }

    public void Tele()
    {
        Invoke("TelePlayer", 1.2f);
    }

    void TelePlayer()
    {
        _telePos = new Vector2(toTeleport.transform.position.x, toTeleport.transform.position.y + 1);
        player.transform.position = _telePos;
    }
}
