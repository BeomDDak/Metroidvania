using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    // 이동관련
    float speed = 1f;
    float dir = 1f;
    Rigidbody2D rigid;
    SpriteRenderer spriteRenderer;
    bool isMoving = true;

    // 방향전환
    float timer = 0f;
    float changeDirTime = 2f;

    int enemyHP = 5;



    void Start()
    {
        rigid = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    //  감지
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            isMoving = false;
            Vector2 direction = (collision.transform.position - transform.position).normalized;
            spriteRenderer.flipX = (direction.x < 0) ? true : false;
            rigid.velocity = direction * speed;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            isMoving = true;
        }
    }

    private void Update()
    {
        if (isMoving)
        {
            timer += Time.deltaTime;

            if (timer >= changeDirTime)
            {
                dir *= -1;
                changeDirTime = Random.Range(2f, 5f);
                timer = 0f;
            }
            spriteRenderer.flipX = dir == 1 ? false : true;

            transform.Translate(Vector2.right * speed * Time.deltaTime * dir);
        }
    }

    // 몸통
    private void OnCollisionEnter2D(Collision2D collision)
    {
        
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        
    }

}
