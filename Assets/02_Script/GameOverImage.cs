using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.SceneManagement;
using TMPro;

public class GameOverImage : MonoBehaviour
{
    Transform oritransform;
    bool canRestart = false;
    public GameObject goRestart;
    Color color;
    bool blinkTxt = false;
    public SpriteMask SpriteMask;
    public TextMeshProUGUI txt;

    int _hp;

    // Start is called before the first frame update
    void Start()
    {
        transform.DOMoveY(750, 7).OnComplete(() => { canRestart = true; });
        oritransform = this.transform;
    }

    // Update is called once per frame
    void Update()
    {
        if(canRestart)
        {
            goRestart.SetActive(true);
            blinkTxt = true;
            if (Input.anyKeyDown)
            {
                SpriteMask.alphaCutoff = 0;
                this.transform.position = oritransform.position;
                this.gameObject.SetActive(false);
                goRestart.SetActive(false);
                _hp = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerDamage>().playerHp;
                _hp = 4;
            }
        }

        if (blinkTxt)
        {
            txt.DOFade(0f, 1).SetLoops(-1);
        }
    }
}
