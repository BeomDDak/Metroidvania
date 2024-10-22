using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttackBall : MonoBehaviour
{
    float speed = 3f;
    Enemy2 _enemy;
    float deleteObj = 3f;
    float deleteTime = 0f;
    int _enemyFlip;
    void Start()
    {
        _enemy = GameObject.FindGameObjectWithTag("Enemy").GetComponent<Enemy2>();
        _enemyFlip = _enemy.GetComponent<SpriteRenderer>().flipX ? 1 : -1;

    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector2.left * speed * Time.deltaTime * _enemyFlip);

        deleteTime += Time.deltaTime;
        if(deleteTime > deleteObj)
        {
            deleteTime = 0f;
            Destroy(gameObject);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Destroy(gameObject);
    }
}
