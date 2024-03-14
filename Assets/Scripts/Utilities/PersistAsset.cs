using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PersistAsset : MonoBehaviour
{
    public bool isDone;

    [Header("广播")]
    public PersistEventSO SavePersistEvent;
    public GameSceneSO gameScene;
    public ForeastTPSetSO foreastTPSet;

    private void OnEnable()
    {
        isDone = true;
        SavePersistEvent.RaiseEvent(isDone, gameScene);     // 广播出去

        if (DataManager.instance.currentGameScene == DataManager.instance.Hive)
            foreastTPSet.RasieEvent();      // 广播
    }

    private void OnDisable()
    {
        isDone = false;
        SavePersistEvent.RaiseEvent(isDone, gameScene);     // 广播出去
    }
}
