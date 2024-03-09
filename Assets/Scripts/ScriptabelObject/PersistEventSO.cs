using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(menuName = "Event/PersistentEventSO")]
public class PersistEventSO : ScriptableObject
{
    public UnityAction<bool, GameSceneSO> onEventRaised;

    public void RaiseEvent(bool isbool, GameSceneSO nextScene)
    {
        onEventRaised?.Invoke(isbool, nextScene);
    }
}
