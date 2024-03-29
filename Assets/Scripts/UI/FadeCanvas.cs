using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class FadeCanvas : MonoBehaviour
{
    [Header("�¼�����")]
    public FadeEventSO fadeEvent;

    public Image fadeImage;


    private void OnEnable()
    {
        fadeEvent.OnEventRaised += OnFadeEvent;
    }

    private void OnDisable()
    {
        fadeEvent.OnEventRaised -= OnFadeEvent;
    }


    private void OnFadeEvent(Color targetcolor, float duration, bool fadeIn, string textName)
    {
        fadeImage.DOBlendableColor(targetcolor, duration);
    }
}

