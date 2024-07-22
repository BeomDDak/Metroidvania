using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttackBall : MonoBehaviour
{
    float speed = 3f;
    Enemy2 _enemy;
    float deleteObj = 5f;
    void Start()
    {
        _enemy = GameObject.FindGameObjectWithTag("Enemy").GetComponent<Enemy2>();
    }

    // Update is called once per frame
    void Update()
    {
        float _enemyFlip = _enemy.GetComponent<SpriteRenderer>().flipX ? -1 : 1;
        transform.Translate(Vector2.left * speed * Time.deltaTime * _enemyFlip);

        float times = 0;
        times += Time.deltaTime;
        if(times > deleteObj)
        {
            Destroy(gameObject);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Destroy(gameObject);
    }
}
