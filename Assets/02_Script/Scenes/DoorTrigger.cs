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
    [SerializeField] private DoorToSpawnAt doorToSpawnAt;
    [SerializeField] private SceneField _sceneToLoad;

    [Space(10f)]
    [Header("This Door")]
    public DoorToSpawnAt CurrentDoorPosition;

    public override void Interact()
    {
        // ¿Ãµø«“ ∏  æ¿
        SceneSwapManager.SwapSceneFromDoorUse(_sceneToLoad, doorToSpawnAt);
    }

}
