using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneSwapManager : MonoBehaviour
{
    public static SceneSwapManager instance;

    private static bool _loadFromDoor;

    // 씬 전환 후 나타날 플레이어 위치
    private GameObject _player;
    private Collider2D _playerColl;
    private Collider2D doorColl;
    private Vector3 _playerSpawnPosition;

    private DoorTrigger.DoorToSpawnAt _doorToSpawnTo;

    private void Awake()
    {
        if( instance == null)
        {
            instance = this;
        }

        _player = GameObject.FindGameObjectWithTag("Player");
        _playerColl = _player.GetComponent<Collider2D>();
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoded;
    }


    public static void SwapSceneFromDoorUse(SceneField myscene, DoorTrigger.DoorToSpawnAt doorToSpawnAt)
    {
        // 스태틱(정적) 이기 때문에 앞에 instance를 붙여줘야함
        // 스태틱(정적) 이기 때문에 참조는 필요하지 않음
        _loadFromDoor = true;
        instance.StartCoroutine(instance.FadeOutThenChangeScene(myscene,doorToSpawnAt));
    }

    // 기본값은 None으로 설정하여 설정하기 전에는 이동되지 않게함
    private IEnumerator FadeOutThenChangeScene(SceneField myScene, DoorTrigger.DoorToSpawnAt doorToSpawn = DoorTrigger.DoorToSpawnAt.None)
    {
        // 화면 검정색 -> 페이드 아웃 -> 문 할당 -> 새 씬 로드

        SceneFadeManager.instance.StartFadeOut();

        while (SceneFadeManager.instance.isFadingOut)
        {
            yield return null;
        }

        _doorToSpawnTo = doorToSpawn;
        SceneManager.LoadSceneAsync(myScene);
    }

    private void OnSceneLoded(Scene scene, LoadSceneMode mode)
    {
        // 시작할때부터 호출되기에 시작은 페이드인으로 시작
        SceneFadeManager.instance.StartFadeIn();
        if (_loadFromDoor)
        {
            FindDoor(_doorToSpawnTo);
            _player.transform.position = _playerSpawnPosition;

            _loadFromDoor = false;
        }
    }

    private void FindDoor(DoorTrigger.DoorToSpawnAt doorSpawnNum)
    {
        DoorTrigger[] doors = FindObjectsOfType<DoorTrigger>();

        for(int i = 0; i < doors.Length; i++)
        {
            if(doors[i].CurrentDoorPosition == doorSpawnNum)
            {
                doorColl = doors[i].gameObject.GetComponent<Collider2D>();

                CalculateSpawnPosition();
                return;
            }
        }
    }

    private void CalculateSpawnPosition()
    {
        float colliderHeight = _playerColl.bounds.extents.y;
        _playerSpawnPosition = doorColl.transform.position - new Vector3(0f, colliderHeight, 0f);
    }
}
