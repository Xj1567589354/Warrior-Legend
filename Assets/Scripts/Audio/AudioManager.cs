using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    [Header("���")]
    public AudioSource bgmSource;       // ��������
    public AudioSource fxSource;        // ������Ч

    [Header("����")]
    public PlayAudioEventSO FXEvent;
    public PlayAudioEventSO BGMEvent;

    private void OnEnable()
    {
        FXEvent.onEventRaised += OnFXEvent;
        BGMEvent.onEventRaised += OnBGMEvent;
    }

    private void OnDisable()
    {
        FXEvent.onEventRaised -= OnFXEvent;
        BGMEvent.onEventRaised -= OnBGMEvent;
    }

    /// <summary>
    /// FX��Ƶ
    /// </summary>
    /// <param name="clip"></param>
    private void OnFXEvent(AudioClip clip)
    {
        fxSource.clip = clip;
        fxSource.Play();
    }

    /// <summary>
    /// BGM��Ƶ
    /// </summary>
    /// <param name="clip"></param>
    private void OnBGMEvent(AudioClip clip)
    {
        bgmSource.clip = clip;
        bgmSource.Play();
    }
}
