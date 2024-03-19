using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextPromt : MonoBehaviour
{
    public FadeEventSO textFadeEvent;
    public FadeEventSO fadeEvent;

    private Color textColor;
    private Color color;

    public GameObject telepoint;

    public bool isTrigger;

    private void OnEnable()
    {
        textColor = new Color(0, 1, 0.7f, 1);
        color = new Color(0, 0, 0, 0.6f);
    }



    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!isTrigger && !telepoint.activeSelf) {
            // ��������ʾ
            fadeEvent.FadeIn(color, 1.5f, "NoT");
            textFadeEvent.FadeIn(textColor, 1.5f, "NoT");
            Invoke("SetCBUI", 3f);
            isTrigger = true;
        }
    }

    /// <summary>
    ///�رմ�������ʾ
    /// </summary>
    void SetCBUI()
    {
        // ��������ʾ
        fadeEvent.FadeOut(1.5f, "NoT");
        textFadeEvent.FadeOut(1.5f, "NoT");
    }
}
