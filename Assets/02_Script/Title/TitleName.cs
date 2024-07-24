using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using TMPro;
public class TitleName : MonoBehaviour
{
    public string gameName;
    public TextMeshProUGUI gameNameTxt;

    void Start()
    {
        transform.DOMoveY(900, 3).OnComplete(() => { StartCoroutine(Typing(0.2f)); });
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    IEnumerator Typing(float intaval)
    {
        int index = 1;
        string description = gameName;
        while (index <= gameName.Length)
        {
            gameNameTxt.text = description.Substring(0, index);
            yield return new WaitForSeconds(intaval);
            index++;
        }
    }
}
