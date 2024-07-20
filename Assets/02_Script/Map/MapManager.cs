using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class MapManager : MonoBehaviour
{
    public static MapManager instance;

    [SerializeField] private GameObject _miniMap;
    [SerializeField] private GameObject _largeMap;

    [SerializeField] private Camera _mapCamera;
    [SerializeField] private Camera _mainCamera;
    [SerializeField] private RawImage _mapImage;

    private Vector2 mapSize;

    // ������ ���� �� �ƿ�
     private float zoomSpeed = 3f;
     private float oriZoom = 5f;
     private float dragSpeed = 3f;

    // �巡��
    private Vector3 dragOrigin;
    private bool isDragging = false;
    private float currentZoom;


    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }

        // ���� ���� �� ��, �� �ݰ� �� ũ�� ���
        CloseLargeMap();
        currentZoom = _mapCamera.orthographicSize;
        CalculateMapSize();
    }

    private void Update()
    {
        // if (�÷��̾� ���꿡�� mŰ ������ bool ���� ���� ������)
        if (PlayerMove.isLargeMap)
        {
            OpenLargeMap();
            HandleMapZoom();
            HandleMapDrag();
            CalculateMapSize();

        }
        else
        {
            CloseLargeMap();
            _mapCamera.orthographicSize = oriZoom;
            _mapCamera.transform.position = _mainCamera.transform.position;
        }
    }

    // �� ���� �ݴ� �Լ�
    void OpenLargeMap()
    {
        _miniMap.SetActive(false);
        _largeMap.SetActive(true);
    }

    void CloseLargeMap()
    {
        _miniMap.SetActive(true);
        _largeMap.SetActive(false);
    }



    // �� ũ�� ���
    private void CalculateMapSize()
    {
        // Raw Image�� ���� ũ�� ���
        Rect rect = _mapImage.rectTransform.rect;
        Vector2 size = new Vector2(rect.width, rect.height);
        Vector2 worldSize = size / _mapImage.canvas.scaleFactor;
        mapSize = worldSize / 2f;
    }


    // �� ��
    private void HandleMapZoom()
    {

        // ���콺 �� ��
        float scrollDelta = Input.mouseScrollDelta.y;
        if (scrollDelta != 0)
        {
            if (currentZoom < oriZoom)
            {
                currentZoom = oriZoom;
            }
            currentZoom -= scrollDelta * zoomSpeed;
            _mapCamera.orthographicSize = currentZoom;
        }
    }

    // �� �巡��
    private void HandleMapDrag()
    {
        if (Input.GetMouseButtonDown(0))
        {
            isDragging = true;
            dragOrigin = _mapCamera.ScreenToWorldPoint(Input.mousePosition);
        }

        if (Input.GetMouseButtonUp(0))
        {
            isDragging = false;
        }

        if (isDragging)
        {
            Vector3 difference = dragOrigin - _mapCamera.ScreenToWorldPoint(Input.mousePosition);
            _mapCamera.transform.position += difference;
        }
    }
}
