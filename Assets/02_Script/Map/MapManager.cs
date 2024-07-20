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

    // 라지맵 줌인 줌 아웃
     private float zoomSpeed = 3f;
     private float oriZoom = 5f;
     private float dragSpeed = 3f;

    // 드래그
    private Vector3 dragOrigin;
    private bool isDragging = false;
    private float currentZoom;


    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }

        // 게임 시작 할 때, 맵 닫고 맵 크기 계산
        CloseLargeMap();
        currentZoom = _mapCamera.orthographicSize;
        CalculateMapSize();
    }

    private void Update()
    {
        // if (플레이어 무브에서 m키 눌러서 bool 값을 전달 받으면)
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

    // 맵 열고 닫는 함수
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



    // 맵 크기 계산
    private void CalculateMapSize()
    {
        // Raw Image의 실제 크기 계산
        Rect rect = _mapImage.rectTransform.rect;
        Vector2 size = new Vector2(rect.width, rect.height);
        Vector2 worldSize = size / _mapImage.canvas.scaleFactor;
        mapSize = worldSize / 2f;
    }


    // 맵 줌
    private void HandleMapZoom()
    {

        // 마우스 휠 값
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

    // 맵 드래그
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
