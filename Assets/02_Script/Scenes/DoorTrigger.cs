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
    [SerializeField] private SceneField _sceneToLoad;
    [SerializeField] private DoorToSpawnAt doorToSpawnAt;
    

    [Space(10f)]
    [Header("This Door")]
    public DoorToSpawnAt CurrentDoorPosition;

    // 이 스크립트를 오브젝트에 달아놓고 값을 받자
    public override void Interact()
    {
        if (SceneSwapManager.isTransitioning) return;

        Debug.Log($"DoorTrigger Interact called. Scene to load: {_sceneToLoad.SceneName}, Door to spawn at: {doorToSpawnAt}");
        // 이동할 맵 씬
        SceneSwapManager.SwapSceneFromDoorUse(_sceneToLoad, doorToSpawnAt);
    }

}
