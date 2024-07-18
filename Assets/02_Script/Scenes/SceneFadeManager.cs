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
                // ���� ���� �ٷ� ���� �Ұ��� -> �����鿡 ���� �Ҵ��ϰ� ��ä�� �־���
                fadeOutStartColor.a += Time.deltaTime * _fadeOutSpeed;
                _fadeOutImage.color = fadeOutStartColor;
            }
            else
            {
                Debug.Log("Fade out completed");
                isFadingOut = false;
                isFadeOutComplete = true;
            }
        }

        // ���̵� ��
        if (isFadingIn)
        {
            if (_fadeOutImage.color.a > 0f)
            {
                fadeOutStartColor.a -= Time.deltaTime * _fadeInSpeed;
                _fadeOutImage.color = fadeOutStartColor;
            }
            else
            {
                Debug.Log("Fade in completed");
                isFadingIn = false;
            }
        }
    }

    // ��ŸƮ ���̵�ƿ�
    public void StartFadeOut()
    {
        Debug.Log("StartFadeOut called");
        _fadeOutImage.color = fadeOutStartColor;
        isFadingOut = true;
        isFadeOutComplete = false;
    }

    // ��ŸƮ ���̵���
    public void StartFadeIn()
    {
        Debug.Log("StartFadeIn called");
        if (isFadeOutComplete)
        {
            _fadeOutImage.color = fadeOutStartColor;
            isFadingIn = true;
        }
        else
        {
            Debug.LogWarning("StartFadeIn called but alpha is not 1");
        }
    }
}
