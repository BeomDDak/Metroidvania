using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using TMPro;

public class PlayerDamage : MonoBehaviour
{
    // 캐릭터 상태 변화 변수
    SpriteRenderer playerColor;
    Color oriColor;
    Color damageColor;
    PlayerMove _playerState;

    // HP 관련 변수
    public GameObject[] heart = new GameObject[5];
    public int playerHp = 4;
    public SpriteMask SpriteMask;
    public GameObject gameOverImage;
    float deadTIme = 0;

    // 재화 변수
    public TextMeshProUGUI jam;

    // 공격 콜라이더
    public GameObject weaponColl1,weaponColl2;

    void Start()
    {
        // 변수 초기화
        playerColor = GetComponent<SpriteRenderer>();
        oriColor = playerColor.color;
        damageColor = Color.red;
        _playerState = GetComponent<PlayerMove>();
    }

    private void Update()
    {
        // 재화 표시
        jam.text = DataManager.instance.jam.ToString("###,###");
        UI_HeartChanges();
    }

    // ============== 피격 ==============
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

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Trap"))
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

    // ============= HP ==============
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
            deadTIme += Time.deltaTime;
            if(deadTIme > 1f)
            {
                SpriteMask.alphaCutoff += Time.deltaTime * 0.5f;
                gameOverImage.SetActive(true);
            }
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

    // ========= 공격 ============
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

    public void StopAttack2()
    {
        weaponColl2.SetActive(false);
    }
}
