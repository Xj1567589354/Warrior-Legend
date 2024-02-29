using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(menuName = "Event/FadeEventSO")]
public class FadeEventSO : ScriptableObject
{

    public UnityAction<Color, float, bool> OnEventRaised;

    /// <summary>
    /// 渐入
    /// </summary>
    /// <param name="duration">时间</param>
    public void FadeIn(float duration)
    {
        RaiseEvent(Color.clear, duration, true);
    }


    /// <summary>
    /// 渐出
    /// </summary>
    /// <param name="duration">时间</param>
    public void FadeOut(float duration) 
    {
        RaiseEvent(Color.black, duration, false);
    }

    public void RaiseEvent(Color color,float duration, bool fadeIn)
    {
        OnEventRaised?.Invoke(color,duration,fadeIn);
    }
}