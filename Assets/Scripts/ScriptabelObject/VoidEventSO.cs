using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(menuName = "Event/VoidEventSO")]
public class VoidEventSO : ScriptableObject
{
    // �㲥�������
    public UnityAction onEventRaised;

    public void RasieEvent()
    {
        onEventRaised?.Invoke();
    }
}
