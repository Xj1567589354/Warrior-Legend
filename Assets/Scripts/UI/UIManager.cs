using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TextCore.Text;
using UnityEngine.EventSystems;

public class UIManager : MonoBehaviour
{
    public PlayerStatBar playerStatBar;
    public BossHealthBar bossHealthBar;
    public Boar boss;
    public Boar blackBoar;
    public Boar redBoar;
    public GameObject telepoint;

    [Header("�¼�����")]
     /*��Ϊ�м��߽��й㲥�ͼ���*/
    public CharactorEventSO healthEvent;
    public CharactorEventSO bossHealthEvent;
    public SceneLoaderEventSO unloadedSceneEvent;       // ����ж�س�����ִ�е��¼�
    public VoidEventSO loadDataEvent;                   // ������Ϸ������ִ�е��¼�
    public VoidEventSO gameOverEvent;                   // ��Ϸ������ִ�е��¼�
    public VoidEventSO backToMenuEvent;             // �������˵���ִ�е��¼�

    [Header("���")]
    public GameObject gameOverPanel;        // �������
    public GameObject restartBtn;           // ������ť

    private void OnEnable()
    {
        bossHealthEvent.OnEventRaised += OnBossHealthEvent;
        healthEvent.OnEventRaised += OnHealthEvent;
        unloadedSceneEvent.LoadRequestEvent += OnUnLoadedSceneEvent;
        loadDataEvent.onEventRaised += OnLoadDataEvent;
        gameOverEvent.onEventRaised += OnGameOverEvent;
        backToMenuEvent.onEventRaised += OnLoadDataEvent;
    }

    private void OnDisable()
    {
        bossHealthEvent.OnEventRaised -= OnBossHealthEvent;
        healthEvent.OnEventRaised -= OnHealthEvent;
        unloadedSceneEvent.LoadRequestEvent -= OnUnLoadedSceneEvent;
        loadDataEvent.onEventRaised -= OnLoadDataEvent;
        gameOverEvent.onEventRaised -= OnGameOverEvent;
        backToMenuEvent.onEventRaised -= OnLoadDataEvent;
    }

    private void Update()
    {
        if (DataManager.instance.currentGameScene == DataManager.instance.Boss
             && !boss.isDead)
        {
            bossHealthBar.gameObject.SetActive(true);
        }

        if (DataManager.instance.currentGameScene == DataManager.instance.Boss
            && boss.isDead)
        {
            if (!blackBoar.isDead && !redBoar.isDead)
            {
                telepoint.SetActive(true);
                blackBoar.OnDie();
                redBoar.OnDie();
                bossHealthBar.gameObject.SetActive(false);
                print("dead");
            }
        }

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

    private void OnBossHealthEvent(Charactor charactor)
    {
        /*int���Ͳ���1���ͻ���ȥС�����֣�ֱ�ӵ���0*/
        var persentage = (float)charactor.currentHealth / charactor.maxHealth;
        bossHealthBar.OnHealthChange(persentage);
    }
}
