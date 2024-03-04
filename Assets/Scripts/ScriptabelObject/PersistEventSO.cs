using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(menuName = "Event/PersistentEventSO")]
public class PersistEventSO : ScriptableObject
{
    public UnityAction<bool> onEventRaised;

    public void RaiseEvent(bool isbool)
    {
        onEventRaised?.Invoke(isbool);
    }
}
