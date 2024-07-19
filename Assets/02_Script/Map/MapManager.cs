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

        // ���� �����Ҷ� �� �ݰ� ����
        CloseLargeMap();
    }

    private void Update()
    {
        // if (�÷��̾� ���꿡�� mŰ ������ bool ���� ���� ������)
        if (!IsLargeMapOpen)
        {
            OpenLargeMap();
        }
        else
        {
            CloseLargeMap();
        }
    }

    // �� ���� �ݴ� �Լ�
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
