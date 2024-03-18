using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoveItem : MonoBehaviour
{
    public PlayAudioEventSO playAudioEventSO;
    public AudioClip audioClip;
    public BoolEventSO boolEvent;
    private bool isLove;


    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            playAudioEventSO.RaiseEvent(audioClip);
            Destroy(gameObject);
            isLove = true;
            boolEvent.RaiseEvent(isLove);
        }
    }
}
