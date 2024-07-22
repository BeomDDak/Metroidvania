using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    // �̵�����
    float speed = 1f;
    float dir = 1f;
    Rigidbody2D rigid;
    SpriteRenderer spriteRenderer;
    bool isMoving = true;
    //public LayerMask groundLayer;

    // ������ȯ
    float timer = 0f;
    float changeDirTime = 2f;

    public int enemyHP = 5;



    void Start()
    {
        rigid = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // ���� ��������
        if (collision.CompareTag("Attack"))
        {
            enemyHP--;
            float knockbackDirection = spriteRenderer.flipX ? 1 : -1;
            transform.position += Vector3.right * knockbackDirection * 30 * Time.deltaTime;     // rigid�� �ٲ��ָ� ������
            if (enemyHP <= 0)
            {
                Destroy(gameObject);
            }
        }
    }


    //  ����
    private void OnTriggerStay2D(Collider2D collision)
    {
        // �÷��̾� ����
        if (collision.CompareTag("Player"))
        {
            isMoving = false;
            Vector2 direction = (collision.transform.position - transform.position).normalized;
            spriteRenderer.flipX = (direction.x < 0) ? true : false;
            rigid.velocity = direction * speed;
        }
    }

    // ���� �����ȿ� �÷��̾� ������ ���ƴٴ�
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            isMoving = true;
        }
    }

    private void Update()
    {
        //RaycastHit2D hit = Physics2D.Raycast(transform.position,Vector3.down,10f,groundLayer);
        //Debug.DrawRay(transform.position,Vector3.down,Color.red);
        //Debug.Log(hit.collider.gameObject.name);

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
}
