using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChange : MonoBehaviour
{
    
    public static SceneChange instance;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
    }

    public int doorNum = 0;


    void Start()
    {
        
    }

    public void ChangeScene(string _sceneName, int _doorNumber)
    {
        doorNum = _doorNumber;
        SceneManager.LoadScene(_sceneName);
    }

}
