using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioDefination : MonoBehaviour
{
    // π„≤•
    public PlayAudioEventSO playAudioEventSO;
    public AudioClip audioClip;
    public bool playOnEnable;

    private void OnEnable()
    {
        if (playOnEnable)
            PlayAudioClip();
    }

    public void PlayAudioClip()
    {
        // ¥´»Î“Ù∆µ
        playAudioEventSO.RaiseEvent(audioClip);
    }
}
