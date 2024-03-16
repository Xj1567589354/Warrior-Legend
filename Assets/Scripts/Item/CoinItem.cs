using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinItem : MonoBehaviour
{
    public PlayAudioEventSO playAudioEventSO;
    public AudioClip audioClip;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            playAudioEventSO.RaiseEvent(audioClip);
            PlayerStatBar.currentCoinQuantity++;
            Destroy(gameObject);
        }
    }
}
