using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SceneFadeManager : MonoBehaviour
{
    public static SceneFadeManager instance;

    [SerializeField] private Image _fadeOutImage;
    [Range(0.1f,10f), SerializeField] private float _fadeOutSpeed = 5f;
    [Range(0.1f,10f), SerializeField] private float _fadeInSpeed = 5f;

    [SerializeField] private Color fadeOutStartColor;

    public bool isFadingOut { get; private set; }
    public bool isFadingIn {  get; private set; }
    public bool isFadeOutComplete { get; private set; }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }

        fadeOutStartColor.a = 0f;
    }

    //  ���̵� ó�� �� ��
    private void Update()
    {
        // ���̵� �ƿ�
        if(isFadingOut)
        {
            if(_fadeOutImage.color.a < 1f)
            {
                fadeOutStartColor.a += Time.deltaTime * _fadeOutSpeed;
                _fadeOutImage.color = fadeOutStartColor;
            }
            else
            {
                isFadingOut = false;
                isFadeOutComplete = true;
            }
        }

        #region ���̵� ��

        if (isFadingIn)
        {
            if (_fadeOutImage.color.a > 0f)
            {
                fadeOutStartColor.a -= Time.deltaTime * _fadeInSpeed;
                _fadeOutImage.color = fadeOutStartColor;
            }
            else
            {
                isFadingIn = false;
            }
        }

        #endregion
    }

    // ��ŸƮ ���̵�ƿ�
    public void StartFadeOut()
    {
        _fadeOutImage.color = fadeOutStartColor;
        isFadingOut = true;
        isFadeOutComplete = false;
    }

    // ��ŸƮ ���̵���
    public void StartFadeIn()
    {
        if (isFadeOutComplete)
        {
            _fadeOutImage.color = fadeOutStartColor;
            isFadingIn = true;
        }
    }
}
