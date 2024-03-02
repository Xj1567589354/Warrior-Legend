using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(menuName = "Event/FadeEventSO")]
public class FadeEventSO : ScriptableObject
{

    public UnityAction<Color, float, bool> OnEventRaised;

    /// <summary>
    /// 逐渐变黑
    /// </summary>
    /// <param name="duration">时间</param>
    public void FadeIn(float duration)
    {
        RaiseEvent(Color.black, duration, true);
    }


    /// <summary>
    /// 逐渐变透明
    /// </summary>
    /// <param name="duration">时间</param>
    public void FadeOut(float duration) 
    {
        RaiseEvent(Color.clear, duration, false);
    }

    public void RaiseEvent(Color color,float duration, bool fadeIn)
    {
        OnEventRaised?.Invoke(color,duration,fadeIn);
    }
}