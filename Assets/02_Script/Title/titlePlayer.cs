using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class titlePlayer : MonoBehaviour
{
    Animator anim;
    float speed = 0;
    bool isWalk = true;

    private void Start()
    {
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        if (isWalk)
        {
            transform.Translate(Vector2.right * speed * Time.deltaTime);
            if (transform.position.x >= 0)
            {
                isWalk = false;
            }
        }
        else if ( !isWalk)
        {
            anim.Play("Idle");
        }
    }
}
