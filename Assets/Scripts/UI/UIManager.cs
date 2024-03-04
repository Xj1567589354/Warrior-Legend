using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TextCore.Text;
using UnityEngine.EventSystems;

public class UIManager : MonoBehaviour
{
    public PlayerStatBar playerStatBar;

    [Header("�¼�����")]
     /*��Ϊ�м��߽��й㲥�ͼ���*/
    public CharactorEventSO healthEvent;
    public SceneLoaderEventSO unloadedSceneEvent;       // ����ж�س�����ִ�е��¼�
    public VoidEventSO loadDataEvent;                   // ������Ϸ������ִ�е��¼�
    public VoidEventSO gameOverEvent;                   // ��Ϸ������ִ�е��¼�
    public VoidEventSO backToMenuEvent;             // �������˵���ִ�е��¼�

    [Header("���")]
    public GameObject gameOverPanel;        // �������
    public GameObject restartBtn;           // ������ť

    private void OnEnable()
    {
        healthEvent.OnEventRaised += OnHealthEvent;
        unloadedSceneEvent.LoadRequestEvent += OnUnLoadedSceneEvent;
        loadDataEvent.onEventRaised += OnLoadDataEvent;
        gameOverEvent.onEventRaised += OnGameOverEvent;
        backToMenuEvent.onEventRaised += OnLoadDataEvent;
    }

    private void OnDisable()
    {
        healthEvent.OnEventRaised -= OnHealthEvent;
        unloadedSceneEvent.LoadRequestEvent -= OnUnLoadedSceneEvent;
        loadDataEvent.onEventRaised -= OnLoadDataEvent;
        gameOverEvent.onEventRaised -= OnGameOverEvent;
        backToMenuEvent.onEventRaised -= OnLoadDataEvent;
    }

    private void OnGameOverEvent()
    {
        gameOverPanel.SetActive(true);
        EventSystem.current.SetSelectedGameObject(restartBtn);      // Ĭ��ѡ�����¿�ʼ��ť
    }

    private void OnLoadDataEvent()
    {
        gameOverPanel.SetActive(false);
    }

    private void OnUnLoadedSceneEvent(GameSceneSO sceneToLoad, Vector3 arg1, bool arg2)
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
