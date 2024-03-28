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
    private bool isBossDead;

    [Header("事件监听")]
     /*作为中间者进行广播和监听*/
    public CharactorEventSO healthEvent;
    public CharactorEventSO bossHealthEvent;
    public SceneLoaderEventSO unloadedSceneEvent;       // 这是卸载场景所执行的事件
    public VoidEventSO loadDataEvent;                   // 加载游戏进度所执行的事件
    public VoidEventSO gameOverEvent;                   // 游戏结束所执行的事件
    public VoidEventSO backToMenuEvent;             // 返回主菜单所执行的事件

    [Header("组件")]
    public GameObject gameOverPanel;        // 结束面板
    public GameObject restartBtn;           // 继续按钮

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
        // 如果当前处于BOSS关卡
        if (DataManager.instance.currentGameScene == DataManager.instance.Boss && !boss.isDead)
        {
            bossHealthBar.gameObject.SetActive(true);       // 打开boss血条
        }

        if (DataManager.instance.currentGameScene == DataManager.instance.Boss && boss.isDead)
        {
            if (!blackBoar.isDead && !redBoar.isDead)
            {
                bossHealthBar.gameObject.SetActive(false);
                telepoint.SetActive(true);
                blackBoar.OnDie();
                redBoar.OnDie();
                print("dead");
            }
        }

        if (DataManager.instance.currentGameScene != DataManager.instance.Boss)
            bossHealthBar.gameObject.SetActive(false);
    }

    private void OnGameOverEvent()
    {
        gameOverPanel.SetActive(true);
        EventSystem.current.SetSelectedGameObject(restartBtn);      // 默认选择重新开始按钮
    }

    private void OnLoadDataEvent()
    {
        gameOverPanel.SetActive(false);
    }

    private void OnUnLoadedSceneEvent(GameSceneSO sceneToLoad, Vector3 arg1, bool arg2)
    {
        // 如果是menu场景，则将血量条设置为false
        var isMenu = sceneToLoad.scenetype == Scenetype.Menu;
        playerStatBar.gameObject.SetActive(!isMenu);
    }

    private void OnHealthEvent(Charactor charactor)
    {
        /*int类型不足1，就会舍去小数部分，直接等于0*/
        var persentage = (float)charactor.currentHealth / charactor.maxHealth;
        playerStatBar.OnHealthChange(persentage);

        playerStatBar.OnPoerChange(charactor);
    }

    private void OnBossHealthEvent(Charactor charactor)
    {
        /*int类型不足1，就会舍去小数部分，直接等于0*/
        var persentage = (float)charactor.currentHealth / charactor.maxHealth;
        bossHealthBar.OnHealthChange(persentage, charactor.currentHealth);
    }
}
