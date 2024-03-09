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

    private void OnEnable()
    {
        isDone = true;
        SavePersistEvent.RaiseEvent(isDone, gameScene);     // �㲥��ȥ
    }

    private void OnDisable()
    {
        isDone = false;
        SavePersistEvent.RaiseEvent(isDone, gameScene);     // �㲥��ȥ
    }
}
