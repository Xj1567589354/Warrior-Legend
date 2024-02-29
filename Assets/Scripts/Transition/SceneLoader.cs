using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceProviders;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    public Transform playerTrans;       // 玩家
    public Vector3 firstPostion;      // 初始坐标
    public Vector3 menuPostion;      

    [Header("事件监听")]
    public SceneLoaderEventSO loadEventSO;
    public VoidEventSO newGameEvent;

    [Header("广播")]
    public VoidEventSO afterSceneLoadedEvent;
    public FadeEventSO fadeEvent;
    public SceneLoaderEventSO sceneUnloadedEvent;

    [Header("场景")]
    public GameSceneSO firstLoadScene;      // 新游戏开始的场景
    public GameSceneSO menuScene;           // 菜单

    private GameSceneSO sceneToLoad;
    private GameSceneSO currentLoadedScene;     // 当前加载的场景
    private Vector3 positionToGo;
    private bool fadeSceen;
    private bool isLoading;     // 是否正在加载

    public float fadeDurationTime;              // 渐入渐出时间

    private void Awake()
    {
        //currentLoadedScene = firstLoadScene;
        //currentLoadedScene.assetReference.LoadSceneAsync(LoadSceneMode.Additive);       // 异步加载场景
    }

    //TODO:MainMenu之后需要修改
    private void Start()
    {
        loadEventSO.RaiseLoadRequest(menuScene, menuPostion, true);
        //NewGame();
    }

    private void OnEnable()
    {
        loadEventSO.LoadRequestEvent += OnLoadRequestEvent;
        newGameEvent.onEventRaised += NewGame;
    }

    private void OnDisable()
    {
        loadEventSO.LoadRequestEvent -= OnLoadRequestEvent;
        newGameEvent.onEventRaised -= NewGame;
    }

    void NewGame()
    {
        sceneToLoad = firstLoadScene;       // 第一个需要加载的场景
        //OnLoadRequestEvent(sceneToLoad, firstPostion, true);
        loadEventSO.RaiseLoadRequest(sceneToLoad, firstPostion, true);
    }

    /// <summary>
    /// 场景加载事件请求
    /// </summary>
    /// <param name="locationToLoad"></param>
    /// <param name="posToGo"></param>
    /// <param name="fadeScreen"></param>
    private void OnLoadRequestEvent(GameSceneSO locationToLoad, Vector3 posToGo, bool fadeScreen)
    {
        // 如果进入新场景，就不如持续按交互E
        if (isLoading)
        {
            return;
        }
        isLoading = true;

        // 暂存监听到的值--要加载的场景，Player目的坐标，是否渐入渐出
        sceneToLoad = locationToLoad;
        positionToGo = posToGo;
        this.fadeSceen = fadeScreen;

        if (currentLoadedScene != null)
            StartCoroutine(UnLoadPreviousScene());      // 开启协程
        else
            LoadNewScene();         // 加载新场景
    }

    /// <summary>
    /// 协程--卸载先前场景
    /// </summary>
    /// <returns></returns>
    private IEnumerator UnLoadPreviousScene()
    {
        if (fadeSceen)
        {
            //TODD:实现渐入渐出
            fadeEvent.FadeOut(fadeDurationTime);
        }

        yield return new WaitForSeconds(fadeDurationTime);

        // 广播事件调整血量显示
        sceneUnloadedEvent.RaiseLoadRequest(sceneToLoad, positionToGo, true);

        yield return currentLoadedScene.assetReference.UnLoadScene();        // 卸载场景

        playerTrans.gameObject.SetActive(false);        // 关闭玩家

        LoadNewScene();     // 加载新场景
    }

    /// <summary>
    /// 加载新场景
    /// </summary>
    private void LoadNewScene()
    {
        var loadingOption = sceneToLoad.assetReference.LoadSceneAsync(LoadSceneMode.Additive, true);
        loadingOption.Completed += OnLoadCompleted;     // 场景加载好之后做的事情
    }

    /// <summary>
    /// 场景加载结束之后
    /// </summary>
    /// <param name="obj"></param>
    private void OnLoadCompleted(AsyncOperationHandle<SceneInstance> obj)
    {
        currentLoadedScene = sceneToLoad;       // 更新当前场景
        playerTrans.position = positionToGo;    // 更新player坐标

        playerTrans.gameObject.SetActive(true);     // 打开玩家
        if (fadeSceen)
        {
            //TODO：渐入渐出
            fadeEvent.FadeIn(fadeDurationTime);
        }

        isLoading = false;

        if (currentLoadedScene.scenetype == Scenetype.Location)
        {
            // 场景加载完成后执行的事件
            afterSceneLoadedEvent.RasieEvent();
        }

    }
}
