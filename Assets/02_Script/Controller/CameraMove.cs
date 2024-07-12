using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMove : MonoBehaviour
{
    public Define.CameraMode _mode = Define.CameraMode.Middle;

    // �÷��̾�κ��� �Ÿ�
    Vector3 cameraPos = new Vector3(0f, 2f, -10f);

    // �÷��̾�
    [SerializeField]
    GameObject _player = null;

    Camera cameraSize;

    // ��ũ�� ����
    public float leftLimit;
    public float rightLimit;
    public float topLimit;
    public float bottomLimit;

    // �޹��
    public GameObject backImage;

    // ��ŷ
    public bool isPeeking = false;
    float peeking = 0;
    float peekingSpeed = 2f;
    float peekingUpDown = 0f;

    // 1�ʰ� ������ �Ÿ�
    public float forceScrollSpeedX = 0.5f;
    public float forceScrollSpeedY = 0.5f;

    // ī�޶� �����̴� ��
    float cameraX;
    float cameraY;

    // �� �̹��� �ʱ� ������
    Vector2 backImagePosition;
    private float previousCameraX;
    private float previousCameraY;




    private void Start()
    {
        cameraSize = GetComponent<Camera>();

        // �޹�� ������ġ�� �ְ� �ϱ�
        if (backImage != null)
        {
            backImagePosition = backImage.transform.position;
        }
        previousCameraX = transform.position.x;
        previousCameraY = transform.position.y;
    }
    void LateUpdate()
    {
        // ī�޶� ������ ����
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

        // �����ǰ� �ʱ�ȭ
        cameraX = _player.transform.position.x;
        cameraY = _player.transform.position.y;

        // ī�޶� �̵� ���� ����
        float limitCameraX = Mathf.Clamp(cameraX, leftLimit, rightLimit);
        float limitCameraY = Mathf.Clamp(cameraY, bottomLimit, topLimit);

        // ī�޶� �̵�
        transform.position = new Vector3(limitCameraX, limitCameraY, -10);

        // ���� ���� �ڵ� ��ũ�� ( ��Ű�� �Ʒ�Ű ������ ������ ī�޶� ��ġ ��������)
        if (isPeeking)
        {
            peeking += transform.position.y + (peekingUpDown * peekingSpeed * Time.deltaTime); // <- ���, ���� �� �Ѱܹ����� �־�����
            peeking = Mathf.Clamp(peeking, -1, 1);
            transform.position = new Vector3(limitCameraX, peeking, -10);
            
        }
        else 
        { 
            transform.position = new Vector3(limitCameraX, limitCameraY, -10);
            
        }

        // @@@@@ �޹�� �̵� @@@@@

        if (backImage != null)
        {
            // �� �̹��� �̵���
            float scrollBackImageSpeedX = 2f;
            float scrollBackImageSpeedY = 2f;
            Vector2 backImageMovement = Vector2.zero;

            // ī�޶� ��ġ�� �Ѱ�ġ�� �����ϸ� �޹�浵 �������� �ʰ� �ϱ�
            if (cameraX >= leftLimit && cameraX <= rightLimit)
            {
                backImageMovement.x = (cameraX - previousCameraX) / scrollBackImageSpeedX;
            }

            if (cameraY >= bottomLimit && cameraY <= topLimit)
            {
                backImageMovement.y = (cameraY - previousCameraY) / scrollBackImageSpeedY;
            }

            // �� �̹����� ������ġ�� += ������ ��
            backImagePosition += backImageMovement;

            // �� �̹����� ��ġ��
            backImage.transform.position = backImagePosition;

            // ���� ī�޶� ��ġ�� �����Ͽ� ���� �����ӿ��� ���
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
