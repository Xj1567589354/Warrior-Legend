using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PersistAsset : MonoBehaviour
{
    public bool isDone;

    [Header("广播")]
    public PersistEventSO SavePersistEvent;

    private void OnEnable()
    {
        isDone = true;
        SavePersistEvent.RaiseEvent(isDone);     // 广播出去
    }

    private void OnDisable()
    {
        isDone = false;
        SavePersistEvent.RaiseEvent(isDone);     // 广播出去
    }
}
