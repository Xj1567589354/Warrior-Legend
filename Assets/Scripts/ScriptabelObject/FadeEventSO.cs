using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(menuName = "Event/FadeEventSO")]
public class FadeEventSO : ScriptableObject
{

    public UnityAction<Color, float, bool, string> OnEventRaised;

    /// <summary>
    /// �𽥱��
    /// </summary>
    /// <param name="duration">ʱ��</param>
    public void FadeIn(Color color, float duration, string textName)
    {
        RaiseEvent(color, duration, true, textName);
    }


    /// <summary>
    /// �𽥱�͸��
    /// </summary>
    /// <param name="duration">ʱ��</param>
    public void FadeOut(float duration, string textName) 
    {
        RaiseEvent(Color.clear, duration, false, textName);
    }

    public void RaiseEvent(Color color,float duration, bool fadeIn, string textName)
    {
        OnEventRaised?.Invoke(color,duration,fadeIn,textName);
    }
}