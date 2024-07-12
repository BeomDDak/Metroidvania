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

    // 스크롤 제한
    public float leftLimit;
    public float rightLimit;
    public float topLimit;
    public float bottomLimit;

    // 뒷배경
    public GameObject backImage;

    // 피킹
    public bool isPeeking = false;
    float peeking = 0;
    float peekingSpeed = 2f;
    float peekingUpDown = 0f;

    // 1초간 움직일 거리
    public float forceScrollSpeedX = 0.5f;
    public float forceScrollSpeedY = 0.5f;

    // 카메라가 움직이는 값
    float cameraX;
    float cameraY;

    // 백 이미지 초기 지정값
    Vector2 backImagePosition;
    private float previousCameraX;
    private float previousCameraY;




    private void Start()
    {
        cameraSize = GetComponent<Camera>();

        // 뒷배경 지정위치에 있게 하기
        if (backImage != null)
        {
            backImagePosition = backImage.transform.position;
        }
        previousCameraX = transform.position.x;
        previousCameraY = transform.position.y;
    }
    void LateUpdate()
    {
        // 카메라 사이즈 변경
        if (_mode == Define.CameraMode.Near)
        {
            cameraSize.orthographicSize = 4;
        }

        if (_mode == Define.CameraMode.Middle)
        {
            cameraSize.orthographicSize = 5;
        }

        if (_mode == Define.CameraMode.Far)
        {
            cameraSize.orthographicSize = 6;
        }

        // 포지션값 초기화
        cameraX = _player.transform.position.x;
        cameraY = _player.transform.position.y;

        // 카메라 이동 범위 제한
        float limitCameraX = Mathf.Clamp(cameraX, leftLimit, rightLimit);
        float limitCameraY = Mathf.Clamp(cameraY, bottomLimit, topLimit);

        // 카메라 이동
        transform.position = new Vector3(limitCameraX, limitCameraY, -10);

        // 세로 강제 자동 스크롤 ( 윗키나 아랫키 누르고 있으면 카메라 위치 변경해줌)
        if (isPeeking)
        {
            peeking += transform.position.y + (peekingUpDown * peekingSpeed * Time.deltaTime); // <- 양수, 음수 값 넘겨받은거 넣어주자
            peeking = Mathf.Clamp(peeking, -1, 1);
            transform.position = new Vector3(limitCameraX, peeking, -10);
            
        }
        else 
        { 
            transform.position = new Vector3(limitCameraX, limitCameraY, -10);
            
        }

        // @@@@@ 뒷배경 이동 @@@@@

        if (backImage != null)
        {
            // 백 이미지 이동값
            float scrollBackImageSpeedX = 2f;
            float scrollBackImageSpeedY = 2f;
            Vector2 backImageMovement = Vector2.zero;

            // 카메라 위치가 한계치에 도달하면 뒷배경도 움직이지 않게 하기
            if (cameraX >= leftLimit && cameraX <= rightLimit)
            {
                backImageMovement.x = (cameraX - previousCameraX) / scrollBackImageSpeedX;
            }

            if (cameraY >= bottomLimit && cameraY <= topLimit)
            {
                backImageMovement.y = (cameraY - previousCameraY) / scrollBackImageSpeedY;
            }

            // 백 이미지의 지정위치값 += 움직인 값
            backImagePosition += backImageMovement;

            // 백 이미지의 위치값
            backImage.transform.position = backImagePosition;

            // 현재 카메라 위치를 저장하여 다음 프레임에서 사용
            previousCameraX = cameraX;
            previousCameraY = cameraY;
        }
    }

    public void GetIsPeeking(bool _isPeeking, float _upAndDown)
    {
        isPeeking = _isPeeking;
        peekingUpDown = _upAndDown;
    }
}
