using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDamage : MonoBehaviour
{
    SpriteRenderer playerColor;
    Color oriColor;
    Color damageColor;
    PlayerMove _playerState;
    Heart _heart;

    void Start()
    {
        playerColor = GetComponent<SpriteRenderer>();
        oriColor = playerColor.color;
        damageColor = Color.red;
        _playerState = GetComponent<PlayerMove>();
        _heart = GetComponent<Heart>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("Æ®¸®°Å");
        if (collision.CompareTag("Trap"))
        {
            StartCoroutine(PlayerDamageAlpha());
            _playerState._state = PlayerMove.PlayerState.Hit;
            _heart.heartHp -= 1;
        }
    }

    IEnumerator PlayerDamageAlpha()
    {
        for(int i = 0; i <= 2; i++)
        {
            playerColor.color = damageColor;
            yield return new WaitForSeconds(0.2f);
            playerColor.color = oriColor;
            yield return new WaitForSeconds(0.2f);
        }
    }
}
