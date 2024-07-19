using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapManager : MonoBehaviour
{
    public static MapManager instance;

    [SerializeField] private GameObject _miniMap;
    [SerializeField] private GameObject _largeMap;

    public bool IsLargeMapOpen { get; private set; }


    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }

        // 게임 시작할땐 맵 닫고 시작
        CloseLargeMap();
    }

    private void Update()
    {
        // if (플레이어 무브에서 m키 눌러서 bool 값을 전달 받으면)
        if (!IsLargeMapOpen)
        {
            OpenLargeMap();
        }
        else
        {
            CloseLargeMap();
        }
    }

    // 맵 열고 닫는 함수
    void OpenLargeMap()
    {
        _miniMap.SetActive(false);
        _largeMap.SetActive(true);
        IsLargeMapOpen = true;

    }

    void CloseLargeMap()
    {
        _miniMap.SetActive(true) ;
        _largeMap.SetActive(false);
        IsLargeMapOpen = false;
    }
}
