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
    [SerializeField] private SceneField _sceneToLoad;           // �̵��� ��
    [SerializeField] private DoorToSpawnAt doorToSpawnAt;       // �̵��� �� ��ȣ
    

    [Space(10f)]
    [Header("This Door")]
    public DoorToSpawnAt CurrentDoorPosition;                   // �ڽ��� �� ��ȣ

    // ������Ʈ�� �޾Ƴ��� ���� ���� ����
    public override void Interact()
    {
        if (SceneSwapManager.isTransitioning) return;

        // �̵��� �� ��
        SceneSwapManager.SwapSceneFromDoorUse(_sceneToLoad, doorToSpawnAt);
    }

}
