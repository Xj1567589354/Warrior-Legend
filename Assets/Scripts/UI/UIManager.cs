using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TextCore.Text;

public class UIManager : MonoBehaviour
{
    public PlayerStatBar playerStatBar;

    [Header("�¼�����")]
     /*��Ϊ�м��߽��й㲥�ͼ���*/
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
        /*int���Ͳ���1���ͻ���ȥС�����֣�ֱ�ӵ���0*/
        var persentage = (float)charactor.currentHealth / charactor.maxHealth;
        playerStatBar.OnHealthChange(persentage);

        playerStatBar.OnPoerChange(charactor);
    }
}
