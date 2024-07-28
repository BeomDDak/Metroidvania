using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanonZ : MonoBehaviour
{
    Vector3 oriScale;
    Quaternion oriRotation;

    float speed = 50f;
    float scaleXY;
    Collider2D coll;

    float randomSpawn;

    void Start()
    {
        coll = GetComponent<CircleCollider2D>();
    }

    void Update()
    {
        if( gameObject.activeSelf == true)
        {
            scaleXY = transform.localScale.x;
            scaleXY += 2 * Time.deltaTime;

            transform.Rotate(Vector3.forward * speed * Time.deltaTime);     // 회전
            transform.localScale = new Vector2(scaleXY, scaleXY);           // 크기

            if (transform.localScale.x >= 10f)
            {
                scaleXY = 10f;
                coll.enabled = true;
                StartCoroutine(DeleteCanon());
            }
        }
    }

    IEnumerator DeleteCanon()
    {
        yield return new WaitForSeconds(2f);
        Destroy(gameObject);
    }
}
