using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Enemy3 : MonoBehaviour
{
    // �̵�����
    float speed = 3f;
    float dir = 1f;
    Rigidbody2D rigid;
    SpriteRenderer spriteRenderer;
    bool isMoving = true;

    // ������ȯ
    float timer = 0f;
    float changeDirTime = 2f;

    // ���ʹ� HP
    public float enemyHP = 1;

    // ���ʹ� ���Ÿ� ����
    public Transform attackPoint;
    public GameObject attackBall;
    bool isAttack = false;
    public float attackCooldown = 2f; // ���� ��ٿ� �ð�
    private float lastAttackTime; // ������ ���� �ð�

    //UI, ���
    public Image HpImage;
    public GameObject breakTile;
    public GameObject skill;
    public Transform[] skill1;
    public Transform[] skill2;
    float skillTime = 0;

    // ����ǰ


    void Start()
    {
        rigid = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        isAttack = false;
        lastAttackTime = -attackCooldown; // ���� ���� �� �ٷ� ������ �� �ֵ��� ����
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // ���� ��������
        if (collision.CompareTag("Attack"))
        {
            enemyHP -= 0.05f;
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

            // ���� ��ٿ� Ȯ�� �� ����
            if (Time.time >= lastAttackTime + attackCooldown)
            {
                Attack();
            }
        }
    }

    void Attack()
    {
        if (attackPoint != null && attackBall != null)
        {
            Instantiate(attackBall, attackPoint.position, attackPoint.rotation);
            lastAttackTime = Time.time; // ������ ���� �ð� ������Ʈ
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
        if (isMoving)
        {
            timer += Time.deltaTime;

            if (timer >= changeDirTime)
            {
                dir *= -1;
                changeDirTime = Random.Range(2f, 10f);
                timer = 0f;
            }
            spriteRenderer.flipX = dir == 1 ? false : true;

            transform.Translate(Vector2.right * speed * Time.deltaTime * dir);
        }

        HpImage.fillAmount = enemyHP;
        if(enemyHP <= 0.5)
        {
            breakTile.SetActive(false);
        }

        skillTime += Time.deltaTime;
        if(skillTime > 10f)
        {
            int canSkill = Random.Range(0, 2);
            if(canSkill == 0)
            {
                for(int i = 0; i < skill1.Length; i++)
                {
                    Instantiate(skill, skill1[i].transform.position, Quaternion.identity);
                }
            }
            else
            {
                for(int i = 0; i < skill2.Length; i++)
                {
                    Instantiate(skill, skill2[i].transform.position, Quaternion.identity);
                }
            }
            skillTime = 0;
        }
    }
}
