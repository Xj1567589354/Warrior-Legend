using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PersistAsset : MonoBehaviour
{
    public bool isDone;

    [Header("�㲥")]
    public PersistEventSO SavePersistEvent;
    public GameSceneSO gameScene;
    public ForeastTPSetSO foreastTPSet;

    private void OnEnable()
    {
        isDone = true;
        SavePersistEvent.RaiseEvent(isDone, gameScene);     // �㲥��ȥ

        if (DataManager.instance.currentGameScene == DataManager.instance.Hive)
            foreastTPSet.RasieEvent();      // �㲥
    }

    private void OnDisable()
    {
        isDone = false;
        SavePersistEvent.RaiseEvent(isDone, gameScene);     // �㲥��ȥ
    }
}
