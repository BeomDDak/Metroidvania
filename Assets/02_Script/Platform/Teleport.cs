using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Teleport : MonoBehaviour
{
    public GameObject TeleportPosition;
    GameObject player;
    bool canTeleport = false;
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
        Vector2 _telePos = new Vector2(TeleportPosition.transform.position.x, TeleportPosition.transform.position.y + 2);
        player.transform.position = _telePos;
    }
    
}
