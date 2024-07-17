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
        // 스태틱(정적) 이기 때문에 앞에 instance를 붙여줘야함
        // 스태틱(정적) 이기 때문에 참조는 필요하지 않음
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
        SceneManager.LoadScene(myScene);

    }


}
