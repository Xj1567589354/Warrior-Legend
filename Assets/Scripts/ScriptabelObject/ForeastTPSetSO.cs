using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(menuName = "Event/ForeastTPSetSO")]
public class ForeastTPSetSO : ScriptableObject
{
    public UnityAction onEventRaised;

    public void RasieEvent()
    {
        onEventRaised?.Invoke();
    }
}
