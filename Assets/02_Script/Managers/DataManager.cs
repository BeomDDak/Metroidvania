using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataManager : MonoBehaviour
{
    public static DataManager instance;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        
    }

    public int jam = 100;
    public int potion = 3;
    public GameObject mapRader;
    public int maxJumpCount = 1;

    public bool stage1Clear = false;
    public bool stage2Clear = false;
    public bool stage3Clear = false;

}
