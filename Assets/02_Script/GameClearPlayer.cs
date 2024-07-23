using System.Collections;
using System.Collections.Generic;
using UnityEngine;


enum state
{
    walk,
    clear,
}

public class GameClearPlayer : MonoBehaviour
{
    Animator anim;
    float speed = 2f;
    state _state;

    public GameObject partyBall;
    public GameObject clearImage;

    void Start()
    {
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {

        switch (_state)
        {
            case state.walk:
                transform.Translate(Vector2.right * speed * Time.deltaTime);
                anim.Play("Walk");
                break;
            case state.clear:
                anim.SetTrigger("Clear");
                StartCoroutine(PartyBall());
                break;
        }

        if(transform.position.x >= 0)
        {
            speed = 0;
            _state = state.clear;
        }
    }

    IEnumerator PartyBall()
    {
        yield return new WaitForSeconds(1f);
        partyBall.SetActive(true);
        yield return new WaitForSeconds(3f);
        partyBall.SetActive(false);
    }
}
