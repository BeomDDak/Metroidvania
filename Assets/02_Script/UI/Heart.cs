using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Heart : MonoBehaviour
{
    public GameObject[] heart = new GameObject[5];
    public int heartHp = 5;

    void Start()
    {

    }

    void Update()
    {
        if( heartHp > 5 )
        {
            heartHp = 5;
        }

        if( heartHp < 0)
        {
            heartHp = 0;
        }

        for(int i = 0; i < heart.Length; i++)
        {
            if(heartHp == i)
            {
                heart[i].SetActive(true);
            }
            else
            {
                heart[i].SetActive(false);
            }
        }
    }
}
