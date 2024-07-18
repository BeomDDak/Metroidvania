using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneSwapManager : MonoBehaviour
{
    public static SceneSwapManager instance;

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
        // ����ƽ(����) �̱� ������ �տ� instance�� �ٿ������
        // ����ƽ(����) �̱� ������ ������ �ʿ����� ����
        _loadFromDoor = true;
        instance.StartCoroutine(instance.FadeOutThenChangeScene(myscene,doorToSpawnAt));
    }

    // �⺻���� None���� �����Ͽ� �����ϱ� ������ �̵����� �ʰ���
    private IEnumerator FadeOutThenChangeScene(SceneField myScene, DoorTrigger.DoorToSpawnAt doorToSpawn = DoorTrigger.DoorToSpawnAt.None)
    {
        // ȭ�� ������ -> ���̵� �ƿ� -> �� �Ҵ� -> �� �� �ε�

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
        // �����Ҷ����� ȣ��Ǳ⿡ ������ ���̵������� ����
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
