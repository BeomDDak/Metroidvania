using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneSwapManager : MonoBehaviour
{
    public static SceneSwapManager instance;

    // ���̵� �ƿ��� �Ϸ�� �Ŀ��� �÷��̾ ����ؼ� ���� ��ȣ�ۿ� �� �� �ִ� ���·� �����־�
    // Interact �޼��尡 �ݺ��� ȣ�� -> �÷��� �ϳ� ���� �߰� ��ȣ�ۿ� ����
    public  static bool isTransitioning = false;
    private static bool _loadFromDoor;

    // �� ��ȯ �� ��Ÿ�� �÷��̾� ��ġ
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


    // SceneManager.sceneLoaded �̺�Ʈ:

    // �� �̺�Ʈ�� ���ο� ���� �ε�� ������ �߻��Ѵ�.
    // �̺�Ʈ�� ��ϵ� ��� �޼������ �� �ε� �Ϸ� �� ȣ��ȴ�.
    // Unity���� ���ο� ���� �ε��� ��, ���� ���� ��� ������Ʈ�� ���ŵȴ�.
    private void OnEnable()
    {
        //+= �����ڸ� ����Ͽ� �̺�Ʈ�� �޼��带 �߰�
        SceneManager.sceneLoaded += OnSceneLoded;
    }

    private void OnDisable()
    {
        // -= �����ڸ� ����Ͽ� �̺�Ʈ���� �޼��带 ����
        SceneManager.sceneLoaded -= OnSceneLoded;
    }

    // �� �̵� ����
    public static void SwapSceneFromDoorUse(SceneField myscene, DoorTrigger.DoorToSpawnAt doorToSpawnAt)
    {
        Debug.Log($"SwapSceneFromDoorUse called. Scene: {myscene.SceneName}, Door: {doorToSpawnAt}");
        // ����ƽ(����) �̱� ������ �տ� instance�� �ٿ������
        // ����ƽ(����) �̱� ������ ������ �ʿ����� ����

        
        isTransitioning = true;
        _loadFromDoor = true;
        // ���̵�ƿ� �ڷ�ƾ ȣ��
        instance.StartCoroutine(instance.FadeOutThenChangeScene(myscene,doorToSpawnAt));
    }

    // �⺻���� None���� �����Ͽ� �����ϱ� ������ �̵����� �ʰ���
    private IEnumerator FadeOutThenChangeScene(SceneField myScene, DoorTrigger.DoorToSpawnAt doorToSpawn = DoorTrigger.DoorToSpawnAt.None)
    {
        Debug.Log("FadeOutThenChangeScene started");
        // ȭ�� ������ -> ���̵� �ƿ� -> �� �Ҵ� -> �� �� �ε�

        // ���̵�ƿ��Ŵ����� ���̵� �ƿ� ȣ��
        SceneFadeManager.instance.StartFadeOut();

        // ���̵�ƿ��� ������ ������ ��ٸ�
        yield return new WaitUntil(() => SceneFadeManager.instance.isFadeOutComplete);

        Debug.Log("Fade out completed");
        _doorToSpawnTo = doorToSpawn;

        // ���̵�
        Debug.Log($"Attempting to load scene: {myScene.SceneName}");
        SceneManager.LoadScene(myScene);

        Debug.Log($"LoadScene called for {myScene.SceneName}");

    }

    // �� �ҷ�������
    private void OnSceneLoded(Scene scene, LoadSceneMode mode)
    {
        Debug.Log($"OnSceneLoaded called. Scene: {scene.name}, LoadSceneMode: {mode}");
        // ���̵� �ƿ��� ������ ���̵��� ����
        if (SceneFadeManager.instance.isFadeOutComplete)
        {
            SceneFadeManager.instance.StartFadeIn();
        }
        else
        {
            // ���̵� �ƿ� �����°� ��ٸ��� �Լ� ȣ��
            Debug.LogWarning("Scene loaded but fade out is not complete. Waiting...");
            StartCoroutine(WaitForFadeOutThenStartFadeIn());
        }

        // �� ã�� ����
        if (_loadFromDoor)
        {
            Debug.Log($"Loading from door. DoorToSpawnTo: {_doorToSpawnTo}");
            // ������ �� ��ȣ�� �� ã��
            FindDoor(_doorToSpawnTo);

            // �÷��̾� ��ġ �̵�
            _player.transform.position = _playerSpawnPosition;
            _loadFromDoor = false;

            ResetAllTriggers();
        }
        isTransitioning = false;
    }

    // ���̵� �� �ϱ����� ���̵� �ƿ��� �ȳ����� ��ٷ���
    private IEnumerator WaitForFadeOutThenStartFadeIn()
    {
        yield return new WaitUntil(() => SceneFadeManager.instance.isFadeOutComplete);
        SceneFadeManager.instance.StartFadeIn();
    }

    // �÷��̾� ã�Ƽ� ����
    private void ResetAllTriggers()
    {
        TriggerBase[] allTriggers = FindObjectsOfType<TriggerBase>();
        foreach (TriggerBase trigger in allTriggers)
        {
            trigger.ResetReferences();
        }
    }

    // �� ã�� ( ĳ���� ��ġ ���� �ϱ� ���� )
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

    // ĳ���� ��ġ -> �� ��ġ�� �̵�
    private void CalculateSpawnPosition()
    {
        float colliderHeight = _playerColl.bounds.extents.y;
        _playerSpawnPosition = doorColl.transform.position - new Vector3(0f, colliderHeight, 0f);
    }
}
