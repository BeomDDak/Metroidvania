using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class BuyState : MonoBehaviour
{

    void Start()
    {
        Vector3 oriScale = transform.localScale;
        // DOTween 시퀀스 생성
        Sequence mySequence = DOTween.Sequence();

        mySequence.Append(transform.DOScale(1.5f, 0.5f));

        mySequence.AppendInterval(0.5f);

        mySequence.Append(transform.DOScale(oriScale * 0f, 0.5f).OnComplete(() => { Destroy(gameObject);}));


        // DOTween 시퀀스 실행
        mySequence.Play();
    }

    // Update is called once per frame
    void Update()
    {

    }
}
