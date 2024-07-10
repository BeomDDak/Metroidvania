using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMove : MonoBehaviour
{
    public Define.CameraMode _mode = Define.CameraMode.Middle;

    // 플레이어로부터 거리
    Vector3 cameraPos = new Vector3(0f, 2f, -10f);

    // 플레이어
    [SerializeField]
    GameObject _player = null;

    Camera cameraSize;

    private void Start()
    {
        cameraSize = GetComponent<Camera>();
    }
    void LateUpdate()
    {
        transform.position = _player.transform.position + cameraPos;

        if (_mode == Define.CameraMode.Near)
        {
            cameraSize.orthographicSize = 4;
        }

        if(_mode == Define.CameraMode.Middle)
        {
            cameraSize.orthographicSize = 5;
        }
        
        if(_mode == Define.CameraMode.Far)
        {
            cameraSize.orthographicSize = 6;
        }
    }
}
