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
    //public LayerMask groundLayer;

    // 방향전환
    float timer = 0f;
    float changeDirTime = 2f;

    // hp
    public int enemyHP = 5;
    Collider2D col;

    // 전리품
    public GameObject jam;

    void Start()
    {
        rigid = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        col = GetComponent<CircleCollider2D>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // 공격 당했을때
        if (collision.CompareTag("Attack"))
        {
            enemyHP--;
            float knockbackDirection = spriteRenderer.flipX ? 1 : -1;
            transform.position += Vector3.right * knockbackDirection * 30 * Time.deltaTime;     // rigid로 바꿔주면 좋을듯
            if (enemyHP <= 0)
            {
                Destroy(col);
                transform.rotation = Quaternion.Euler(0,0,90);
                Instantiate(jam,transform.position,Quaternion.identity);
                DataManager.instance.jam += 10;
                Invoke("EnemyDie", 1f);
            }
        }
    }

    void EnemyDie()
    {
        Destroy(gameObject);
    }

    //  감지
    private void OnTriggerStay2D(Collider2D collision)
    {
        // 플레이어 감지
        if (collision.CompareTag("Player"))
        {
            isMoving = false;
            Vector2 direction = (collision.transform.position - transform.position).normalized;
            spriteRenderer.flipX = (direction.x < 0) ? true : false;
            rigid.velocity = direction * speed;
        }
    }

    // 감지 범위안에 플레이어 없으며 돌아다님
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
