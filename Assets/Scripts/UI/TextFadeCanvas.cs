using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;

public class TextFadeCanvas : MonoBehaviour
{
    [Header("ÊÂ¼þ¼àÌý")]
    public FadeEventSO fadeEvent;

    public TextMeshProUGUI textY;
    public TextMeshProUGUI textN;


    private void OnEnable()
    {
        fadeEvent.OnEventRaised += OnTextFadeEvent;
    }

    private void OnDisable()
    {
        fadeEvent.OnEventRaised -= OnTextFadeEvent;
    }


    private void OnTextFadeEvent(Color targetcolor, float duration, bool fadeIn, string textName)
    {
        switch (textName)
        {
            case "YesT":
                textY.DOBlendableColor(targetcolor, duration);
                break;
            case "NoT":
                textN.DOBlendableColor(targetcolor, duration);
                break;
        }
    }
}
