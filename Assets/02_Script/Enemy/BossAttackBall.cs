using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossAttackBall : MonoBehaviour
{
    float speed = 5f;
    Enemy3 _enemy;
    float deleteObj = 5f;
    float deleteTime = 0f;
    int _enemyFlip;
    void Start()
    {
        _enemy = GameObject.FindGameObjectWithTag("Enemy").GetComponent<Enemy3>();
        _enemyFlip = _enemy.GetComponent<SpriteRenderer>().flipX ? 1 : -1;

    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector2.left * speed * Time.deltaTime * _enemyFlip);

        deleteTime += Time.deltaTime;
        if (deleteTime > deleteObj)
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
