using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneSwapManager : MonoBehaviour
{
    public static SceneSwapManager instance;

    private DoorTrigger.DoorToSpawnAt _doorToSpawnTo;

    private void Awake()
    {
        if( instance == null)
        {
            instance = this;
        }
    }

    public static void SwapSceneFromDoorUse(SceneField myscene, DoorTrigger.DoorToSpawnAt doorToSpawnAt)
    {
        // ����ƽ(����) �̱� ������ �տ� instance�� �ٿ������
        // ����ƽ(����) �̱� ������ ������ �ʿ����� ����
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
        SceneManager.LoadScene(myScene);

    }


}
