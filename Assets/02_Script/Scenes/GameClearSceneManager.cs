using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameClearSceneManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        DontDestroy dontDestroyObject = FindObjectOfType<DontDestroy>();
        if (dontDestroyObject != null)
        {
            Destroy(dontDestroyObject.gameObject);
        }
    }
}
