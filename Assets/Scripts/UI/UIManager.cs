using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TextCore.Text;

public class UIManager : MonoBehaviour
{
    public PlayerStatBar playerStatBar;

    [Header("事件监听")]
     /*作为中间者进行广播和监听*/
    public CharactorEventSO healthEvent;

    private void OnEnable()
    {
        healthEvent.OnEventRaised += OnHealthEvent;
    }

    private void OnDisable()
    {
        healthEvent.OnEventRaised -= OnHealthEvent;
    }

    private void OnHealthEvent(Charactor charactor)
    {
        /*int类型不足1，就会舍去小数部分，直接等于0*/
        var persentage = (float)charactor.currentHealth / charactor.maxHealth;
        playerStatBar.OnHealthChange(persentage);

        playerStatBar.OnPoerChange(charactor);
    }
}
