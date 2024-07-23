using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Elevetor : MonoBehaviour
{
    public enum ElevatorState
    {
        up,
        down,
    }

    ElevatorState state;
    [Header("������Ʈ")]
    [SerializeField] private SpriteRenderer chainR;
    [SerializeField] private SpriteRenderer chainL;
    [SerializeField] private GameObject floor;

    [Space(10f)]
    [Header("���������� ��")]
    [SerializeField] private float speed = 2f;
    [SerializeField] private float minSize = 2f;
    [SerializeField] private float maxSize = 5f;

    // ü�� ������ x �� (����)
    private double chainX = 0.09375;
    // ���������� ��ž��ġ
    bool isMoving = true;

    void Start()
    {
        // �ʱ� ���������� ����
        state = ElevatorState.down;
    }

    void Update()
    {
        switch (state)
        {
            case ElevatorState.down:
                DownElevator();
                break;

            case ElevatorState.up:
                UpElevator();
                break;
        }
    }
    void DownElevator()
    {
        if(isMoving)
        {
            // �� ����
            float chainY = chainR.size.y;
            chainY += speed * Time.deltaTime;
            float floorY = speed * Time.deltaTime;

            // �� ����
            chainR.size = new Vector2((float)chainX, chainY);
            chainL.size = new Vector2((float)chainX, chainY);
            floor.transform.Translate(Vector2.down * floorY);

            // ����
            if (chainR.size.y >= maxSize)
            {
                StartCoroutine(StopElevator());
            }
        }
    }

    void UpElevator()
    {
        if (isMoving)
        {
            // �� ����
            float chainY = chainR.size.y;
            chainY -= speed * Time.deltaTime;

            float floorY = speed * Time.deltaTime;
            // �� ����
            chainR.size = new Vector2((float)chainX, chainY);
            chainL.size = new Vector2((float)chainX, chainY);
            floor.transform.Translate(Vector2.up * floorY);

            // ����
            if (chainR.size.y <= minSize)
            {
                StartCoroutine(StopElevator());
            }
        }
    }

    IEnumerator StopElevator()
    {
        // ����
        isMoving = !isMoving;
        // 1�ʰ�
        yield return new WaitForSeconds(1f);

        // �ٸ� ���·� ����
        if(state == ElevatorState.down)
        {
            state = ElevatorState.up;
        }

        else if(state == ElevatorState.up)
        {
            state = ElevatorState.down;
        }
        // ������
        isMoving = !isMoving;
    }
}
