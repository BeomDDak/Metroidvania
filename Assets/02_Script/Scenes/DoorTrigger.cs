using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorTrigger : TriggerBase
{
    public enum DoorToSpawnAt
    {
        None,
        One,
        Two,
        Three,
        Four,
    }

    [Header("Spawn To")]
    [SerializeField] private SceneField _sceneToLoad;           // 이동할 씬
    [SerializeField] private DoorToSpawnAt doorToSpawnAt;       // 이동할 문 번호
    

    [Space(10f)]
    [Header("This Door")]
    public DoorToSpawnAt CurrentDoorPosition;                   // 자신의 문 번호

    // 오브젝트에 달아놓고 값을 직접 지정
    public override void Interact()
    {
        if (SceneSwapManager.isTransitioning) return;

        // 이동할 맵 씬
        SceneSwapManager.SwapSceneFromDoorUse(_sceneToLoad, doorToSpawnAt);
    }

}
