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
    public SceneLoaderEventSO loadEvent;

    private void OnEnable()
    {
        healthEvent.OnEventRaised += OnHealthEvent;
        loadEvent.LoadRequestEvent += OnLoadEvent;
    }

    private void OnDisable()
    {
        healthEvent.OnEventRaised -= OnHealthEvent;
        loadEvent.LoadRequestEvent -= OnLoadEvent;
    }

    private void OnLoadEvent(GameSceneSO sceneToLoad, Vector3 arg1, bool arg2)
    {
        // �����menu��������Ѫ��������Ϊfalse
        var isMenu = sceneToLoad.scenetype == Scenetype.Menu;
        playerStatBar.gameObject.SetActive(!isMenu);
    }

    private void OnHealthEvent(Charactor charactor)
    {
        /*int���Ͳ���1���ͻ���ȥС�����֣�ֱ�ӵ���0*/
        var persentage = (float)charactor.currentHealth / charactor.maxHealth;
        playerStatBar.OnHealthChange(persentage);

        playerStatBar.OnPoerChange(charactor);
    }
}
