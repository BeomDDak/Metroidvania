using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Jam : MonoBehaviour
{
    Rigidbody2D rigid;

    void Start()
    {
        rigid = GetComponent<Rigidbody2D>();

        rigid.velocity = Vector2.up * 3;
        Invoke("DeleteJam", 1f);
    }

    void DeleteJam()
    {
        Destroy(gameObject);
    }

}
