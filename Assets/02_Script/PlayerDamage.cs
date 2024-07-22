using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using TMPro;

public class PlayerDamage : MonoBehaviour
{
    SpriteRenderer playerColor;
    Color oriColor;
    Color damageColor;
    PlayerMove _playerState;

    public GameObject[] heart = new GameObject[5];
    public int playerHp = 4;

    public TextMeshProUGUI jam;

    public GameObject weaponColl1,weaponColl2;

    void Start()
    {
        playerColor = GetComponent<SpriteRenderer>();
        oriColor = playerColor.color;
        damageColor = Color.red;
        _playerState = GetComponent<PlayerMove>();
    }

    private void Update()
    {
        jam.text = DataManager.instance.jam.ToString("###,###");
        UI_HeartChanges();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Trap"))
        {
            StartCoroutine(PlayerDamageEffect());
            _playerState._state = PlayerMove.PlayerState.Hit;
            playerHp--;
        }

        if (collision.gameObject.CompareTag("Enemy"))
        {
            StartCoroutine(PlayerDamageEffect());
            _playerState._state = PlayerMove.PlayerState.Hit;
            playerHp--;
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
        if (playerHp > 4)
        {
            playerHp = 4;
        }

        if (playerHp <= 0)
        {
            playerHp = 0;
            _playerState._state = PlayerMove.PlayerState.Die;
        }

        for (int i = 0; i < heart.Length; i++)
        {
            if (playerHp == i)
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

    public void Attack1()
    {
        weaponColl1.SetActive(true);
    }

    public void StopAttack1()
    {
        weaponColl1.SetActive(false);
    }

    public void Attack2()
    {
        weaponColl2.SetActive(true);
    }
}
