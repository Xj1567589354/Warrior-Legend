using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(menuName = "Event/CoinEventSO")]
public class CoinEventSO : ScriptableObject
{
    // �㲥�������
    public UnityAction<GameObject> onEventRaised;

    public void RasieEvent(GameObject gameObject)
    {
        onEventRaised?.Invoke(gameObject);
    }
}
