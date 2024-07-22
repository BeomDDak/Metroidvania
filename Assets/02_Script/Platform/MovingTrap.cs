using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class MovingTrap : MonoBehaviour
{
    public float speed = 2f;
    public float checkDistance = 0.1f;
    public Vector2 trapSize = new Vector2(0.9f, 0.9f);

    private Vector2[] directions = new Vector2[] { Vector2.up, Vector2.right, Vector2.down, Vector2.left };
    private int currentDirectionIndex = 0;

    private void Update()
    {
        Move();
        CheckForDirectionChange();
    }

    private void Move()
    {
        Vector2 movement = directions[currentDirectionIndex] * speed * Time.deltaTime;
        transform.Translate(movement);
    }

    private void CheckForDirectionChange()
    {
        Vector2 currentDirection = directions[currentDirectionIndex];

        // 현재 방향으로 충돌 체크
        bool hitCurrent = CheckCollision(currentDirection);

        // 디버그 레이 그리기
        Debug.DrawRay(transform.position, currentDirection * checkDistance, hitCurrent ? Color.red : Color.green);

        // 현재 방향에 충돌이 있으면 다음 방향으로 전환
        if (hitCurrent)
        {
            currentDirectionIndex = (currentDirectionIndex + 1) % 4;
        }
    }

    private bool CheckCollision(Vector2 direction)
    {
        Vector2 position = (Vector2)transform.position + direction * checkDistance;

        // 함정의 네 모서리에서 충돌 체크
        return Physics2D.OverlapBox(position + new Vector2(trapSize.x / 2, trapSize.y / 2), trapSize, 0f, LayerMask.GetMask("TrapTile")) ||
               Physics2D.OverlapBox(position + new Vector2(-trapSize.x / 2, trapSize.y / 2), trapSize, 0f, LayerMask.GetMask("TrapTile")) ||
               Physics2D.OverlapBox(position + new Vector2(trapSize.x / 2, -trapSize.y / 2), trapSize, 0f, LayerMask.GetMask("TrapTile")) ||
               Physics2D.OverlapBox(position + new Vector2(-trapSize.x / 2, -trapSize.y / 2), trapSize, 0f, LayerMask.GetMask("TrapTile"));
    }
}
