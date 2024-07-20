using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CineCameraMove : MonoBehaviour
{
    [SerializeField] private CinemachineConfiner cineCam;

    private void OnEnable()
    {
        SceneManager.sceneLoaded += onSceneLoaded;
        UpdateCameraConfiner();
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= onSceneLoaded;
    }

    void onSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        UpdateCameraConfiner();
    }


    void UpdateCameraConfiner()
    {
        cineCam = GetComponent<CinemachineConfiner>();
        GameObject cameraRangeObj = GameObject.FindGameObjectWithTag("CameraRange");
        Collider2D cameraRange = cameraRangeObj.GetComponent<Collider2D>();
        cineCam.m_BoundingShape2D = cameraRange;
    }

}
