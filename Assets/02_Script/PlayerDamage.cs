using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerDamage : MonoBehaviour
{
    SpriteRenderer playerColor;
    Color oriColor;
    Color damageColor;
    PlayerMove _playerState;

    public GameObject[] heart = new GameObject[5];
    int heartHp = 4;

    void Start()
    {
        playerColor = GetComponent<SpriteRenderer>();
        oriColor = playerColor.color;
        damageColor = Color.red;
        _playerState = GetComponent<PlayerMove>();
    }

    private void Update()
    {
        UI_HeartChanges();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("트리거");
        if (collision.CompareTag("Trap"))
        {
            StartCoroutine(PlayerDamageEffect());
            _playerState._state = PlayerMove.PlayerState.Hit;
            HeartCalc(-1);
        }
    }

    // 캐릭터 데미지 이펙트
    IEnumerator PlayerDamageEffect()
    {
        for(int i = 0; i <= 2; i++)
        {
            playerColor.color = damageColor;
            yield return new WaitForSeconds(0.2f);
            playerColor.color = oriColor;
            yield return new WaitForSeconds(0.2f);
        }
    }

    void UI_HeartChanges()
    {
        if (heartHp > 5)
        {
            heartHp = 5;
        }

        if (heartHp < 0)
        {
            heartHp = 0;
            _playerState._state = PlayerMove.PlayerState.Die;
        }

        for (int i = 0; i < heart.Length; i++)
        {
            if (heartHp == i)
            {
                heart[i].SetActive(true);
            }
            else
            {
                heart[i].SetActive(false);
                heart[0].SetActive(true);
            }
        }
    }

    public int HeartCalc(int _changeCalc)
    {
        heartHp = heartHp + _changeCalc;
        return heartHp;
    }
}
