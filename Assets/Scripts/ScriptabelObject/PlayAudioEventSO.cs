using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(menuName = "Event/PlayAudioEventSO")]
public class PlayAudioEventSO : ScriptableObject
{
    public UnityAction<AudioClip> onEventRaised;

    public void RaiseEvent(AudioClip clip)
    {
        onEventRaised?.Invoke(clip);
    }
}
