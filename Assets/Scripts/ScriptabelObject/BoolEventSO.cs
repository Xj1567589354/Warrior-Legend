using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(menuName = "Event/BoolEventSO")]
public class BoolEventSO : ScriptableObject
{
    public UnityAction<bool> onEventRaised;

    public void RaiseEvent(bool isbool)
    {
        onEventRaised?.Invoke(isbool);
    }
}
