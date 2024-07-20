using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveFloor : MonoBehaviour
{
    enum MoveState
    {
        left,
        right,
    }
    MoveState state;

    [SerializeField] private float speed = 2f;
    [SerializeField] private float maxLeft = 2f;
    [SerializeField] private float maxRight = 7f;

    bool isMoving = true;

    private void Start()
    {
        state = MoveState.left;
    }

    void Update()
    {
        if (isMoving)
        {
            switch (state)
            {
                case MoveState.left:
                    MoveLeft(); 
                    break;
                case MoveState.right: 
                    MoveRight(); 
                    break;
            }
        }
    }

    void MoveLeft()
    {
        transform.Translate(Vector2.left * speed * Time.deltaTime);
        if(transform.position.x <= maxLeft)
        StartCoroutine(Stop());
    }

    void MoveRight()
    {
        transform.Translate(Vector2.right * speed * Time.deltaTime);
        if(transform.position.x >= maxRight)
        StartCoroutine(Stop());
    }


    IEnumerator Stop()
    {
        isMoving = !isMoving;
        yield return new WaitForSeconds(1f);
        if(state == MoveState.left)
        {
            state = MoveState.right;
        }
        else if (state == MoveState.right)
        {
            state = MoveState.left;
        }
        isMoving = !isMoving;
    }

}
