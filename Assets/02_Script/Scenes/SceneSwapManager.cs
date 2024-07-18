using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneSwapManager : MonoBehaviour
{
    public static SceneSwapManager instance;

    // 페이드 아웃이 완료된 후에도 플레이어가 계속해서 문과 상호작용 할 수 있는 상태로 남아있어
    // Interact 메서드가 반복적 호출 -> 플래그 하나 만들어서 추가 상호작용 방지
    public  static bool isTransitioning = false;
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


    // SceneManager.sceneLoaded 이벤트:

    // 이 이벤트는 새로운 씬이 로드될 때마다 발생한다.
    // 이벤트에 등록된 모든 메서드들이 씬 로드 완료 시 호출된다.
    // Unity에서 새로운 씬을 로드할 때, 기존 씬의 모든 오브젝트가 제거된다.
    private void OnEnable()
    {
        //+= 연산자를 사용하여 이벤트에 메서드를 추가
        SceneManager.sceneLoaded += OnSceneLoded;
    }

    private void OnDisable()
    {
        // -= 연산자를 사용하여 이벤트에서 메서드를 제거
        SceneManager.sceneLoaded -= OnSceneLoded;
    }

    // 맵 이동 시작
    public static void SwapSceneFromDoorUse(SceneField myscene, DoorTrigger.DoorToSpawnAt doorToSpawnAt)
    {
        Debug.Log($"SwapSceneFromDoorUse called. Scene: {myscene.SceneName}, Door: {doorToSpawnAt}");
        // 스태틱(정적) 이기 때문에 앞에 instance를 붙여줘야함
        // 스태틱(정적) 이기 때문에 참조는 필요하지 않음

        
        isTransitioning = true;
        _loadFromDoor = true;
        // 페이드아웃 코루틴 호출
        instance.StartCoroutine(instance.FadeOutThenChangeScene(myscene,doorToSpawnAt));
    }

    // 기본값은 None으로 설정하여 설정하기 전에는 이동되지 않게함
    private IEnumerator FadeOutThenChangeScene(SceneField myScene, DoorTrigger.DoorToSpawnAt doorToSpawn = DoorTrigger.DoorToSpawnAt.None)
    {
        Debug.Log("FadeOutThenChangeScene started");
        // 화면 검정색 -> 페이드 아웃 -> 문 할당 -> 새 씬 로드

        // 페이드아웃매니저에 페이드 아웃 호출
        SceneFadeManager.instance.StartFadeOut();

        // 페이드아웃이 끝나기 전까지 기다림
        yield return new WaitUntil(() => SceneFadeManager.instance.isFadeOutComplete);

        Debug.Log("Fade out completed");
        _doorToSpawnTo = doorToSpawn;

        // 씬이동
        Debug.Log($"Attempting to load scene: {myScene.SceneName}");
        SceneManager.LoadScene(myScene);

        Debug.Log($"LoadScene called for {myScene.SceneName}");

    }

    // 씬 불러와지면
    private void OnSceneLoded(Scene scene, LoadSceneMode mode)
    {
        Debug.Log($"OnSceneLoaded called. Scene: {scene.name}, LoadSceneMode: {mode}");
        // 페이드 아웃이 끝나면 페이드인 시작
        if (SceneFadeManager.instance.isFadeOutComplete)
        {
            SceneFadeManager.instance.StartFadeIn();
        }
        else
        {
            // 페이드 아웃 끝나는거 기다리는 함수 호출
            Debug.LogWarning("Scene loaded but fade out is not complete. Waiting...");
            StartCoroutine(WaitForFadeOutThenStartFadeIn());
        }

        // 문 찾기 시작
        if (_loadFromDoor)
        {
            Debug.Log($"Loading from door. DoorToSpawnTo: {_doorToSpawnTo}");
            // 지정한 문 번호로 문 찾기
            FindDoor(_doorToSpawnTo);

            // 플레이어 위치 이동
            _player.transform.position = _playerSpawnPosition;
            _loadFromDoor = false;

            ResetAllTriggers();
        }
        isTransitioning = false;
    }

    // 페이드 인 하기전에 페이드 아웃이 안끝나면 기다려라
    private IEnumerator WaitForFadeOutThenStartFadeIn()
    {
        yield return new WaitUntil(() => SceneFadeManager.instance.isFadeOutComplete);
        SceneFadeManager.instance.StartFadeIn();
    }

    // 플레이어 찾아서 지정
    private void ResetAllTriggers()
    {
        TriggerBase[] allTriggers = FindObjectsOfType<TriggerBase>();
        foreach (TriggerBase trigger in allTriggers)
        {
            trigger.ResetReferences();
        }
    }

    // 문 찾기 ( 캐릭터 위치 지정 하기 위해 )
    private void FindDoor(DoorTrigger.DoorToSpawnAt doorSpawnNum)
    {
        Debug.Log($"FindDoor called. Looking for door: {doorSpawnNum}");
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
        Debug.LogWarning($"Door not found for position: {doorSpawnNum}");
    }

    // 캐릭터 위치 -> 문 위치로 이동
    private void CalculateSpawnPosition()
    {
        float colliderHeight = _playerColl.bounds.extents.y;
        _playerSpawnPosition = doorColl.transform.position - new Vector3(0f, colliderHeight, 0f);
    }
}
