using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(menuName = "Event/VoidEventSO")]
public class VoidEventSO : ScriptableObject
{
    // ¹ã²¥ÉãÏñ»úÕñ¶¯
    public UnityAction onEventRaised;

    public void RasieEvent()
    {
        onEventRaised?.Invoke();
    }
}
