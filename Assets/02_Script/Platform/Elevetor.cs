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
    [Header("오브젝트")]
    [SerializeField] private SpriteRenderer chainR;
    [SerializeField] private SpriteRenderer chainL;
    [SerializeField] private GameObject floor;

    [Space(10f)]
    [Header("엘리베이터 값")]
    [SerializeField] private float speed = 2f;
    [SerializeField] private float minSize = 2f;
    [SerializeField] private float maxSize = 5f;

    // 체인 사이즈 x 값 (고정)
    private double chainX = 0.09375;
    // 엘리베이터 스탑장치
    bool isMoving = true;

    void Start()
    {
        // 초기 엘리베이터 상태
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
            // 값 대입
            float chainY = chainR.size.y;
            chainY += speed * Time.deltaTime;
            float floorY = speed * Time.deltaTime;

            // 값 변경
            chainR.size = new Vector2((float)chainX, chainY);
            chainL.size = new Vector2((float)chainX, chainY);
            floor.transform.Translate(Vector2.down * floorY);

            // 멈춤
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
            // 값 대입
            float chainY = chainR.size.y;
            chainY -= speed * Time.deltaTime;

            float floorY = speed * Time.deltaTime;
            // 값 변경
            chainR.size = new Vector2((float)chainX, chainY);
            chainL.size = new Vector2((float)chainX, chainY);
            floor.transform.Translate(Vector2.up * floorY);

            // 멈춤
            if (chainR.size.y <= minSize)
            {
                StartCoroutine(StopElevator());
            }
        }
    }

    IEnumerator StopElevator()
    {
        // 멈춤
        isMoving = !isMoving;
        // 1초간
        yield return new WaitForSeconds(1f);

        // 다른 상태로 변경
        if(state == ElevatorState.down)
        {
            state = ElevatorState.up;
        }

        else if(state == ElevatorState.up)
        {
            state = ElevatorState.down;
        }
        // 움직임
        isMoving = !isMoving;
    }
}
