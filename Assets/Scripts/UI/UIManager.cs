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
}
