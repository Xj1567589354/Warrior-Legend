using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PersistAsset : MonoBehaviour
{
    public bool isDone;

    [Header("�㲥")]
    public PersistEventSO SavePersistEvent;

    private void OnEnable()
    {
        isDone = true;
        SavePersistEvent.RaiseEvent(isDone);     // �㲥��ȥ
    }

    private void OnDisable()
    {
        isDone = false;
        SavePersistEvent.RaiseEvent(isDone);     // �㲥��ȥ
    }
}
