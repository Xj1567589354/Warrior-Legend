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
    public Transform playerTrans;       // ���
    public Vector3 firstPostion;      // ��ʼ����
    public Vector3 menuPostion;      

    [Header("�¼�����")]
    public SceneLoaderEventSO loadEventSO;
    public VoidEventSO newGameEvent;

    [Header("�㲥")]
    public VoidEventSO afterSceneLoadedEvent;
    public FadeEventSO fadeEvent;
    public SceneLoaderEventSO sceneUnloadedEvent;

    [Header("����")]
    public GameSceneSO firstLoadScene;      // ����Ϸ��ʼ�ĳ���
    public GameSceneSO menuScene;           // �˵�

    private GameSceneSO sceneToLoad;
    private GameSceneSO currentLoadedScene;     // ��ǰ���صĳ���
    private Vector3 positionToGo;
    private bool fadeSceen;
    private bool isLoading;     // �Ƿ����ڼ���

    public float fadeDurationTime;              // ���뽥��ʱ��

    private void Awake()
    {
        //currentLoadedScene = firstLoadScene;
        //currentLoadedScene.assetReference.LoadSceneAsync(LoadSceneMode.Additive);       // �첽���س���
    }

    //TODO:MainMenu֮����Ҫ�޸�
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
        sceneToLoad = firstLoadScene;       // ��һ����Ҫ���صĳ���
        //OnLoadRequestEvent(sceneToLoad, firstPostion, true);
        loadEventSO.RaiseLoadRequest(sceneToLoad, firstPostion, true);
    }

    /// <summary>
    /// ���������¼�����
    /// </summary>
    /// <param name="locationToLoad"></param>
    /// <param name="posToGo"></param>
    /// <param name="fadeScreen"></param>
    private void OnLoadRequestEvent(GameSceneSO locationToLoad, Vector3 posToGo, bool fadeScreen)
    {
        // ��������³������Ͳ������������E
        if (isLoading)
        {
            return;
        }
        isLoading = true;

        // �ݴ��������ֵ--Ҫ���صĳ�����PlayerĿ�����꣬�Ƿ��뽥��
        sceneToLoad = locationToLoad;
        positionToGo = posToGo;
        this.fadeSceen = fadeScreen;

        if (currentLoadedScene != null)
            StartCoroutine(UnLoadPreviousScene());      // ����Э��
        else
            LoadNewScene();         // �����³���
    }

    /// <summary>
    /// Э��--ж����ǰ����
    /// </summary>
    /// <returns></returns>
    private IEnumerator UnLoadPreviousScene()
    {
        if (fadeSceen)
        {
            //TODD:ʵ�ֽ��뽥��
            fadeEvent.FadeOut(fadeDurationTime);
        }

        yield return new WaitForSeconds(fadeDurationTime);

        // �㲥�¼�����Ѫ����ʾ
        sceneUnloadedEvent.RaiseLoadRequest(sceneToLoad, positionToGo, true);

        yield return currentLoadedScene.assetReference.UnLoadScene();        // ж�س���

        playerTrans.gameObject.SetActive(false);        // �ر����

        LoadNewScene();     // �����³���
    }

    /// <summary>
    /// �����³���
    /// </summary>
    private void LoadNewScene()
    {
        var loadingOption = sceneToLoad.assetReference.LoadSceneAsync(LoadSceneMode.Additive, true);
        loadingOption.Completed += OnLoadCompleted;     // �������غ�֮����������
    }

    /// <summary>
    /// �������ؽ���֮��
    /// </summary>
    /// <param name="obj"></param>
    private void OnLoadCompleted(AsyncOperationHandle<SceneInstance> obj)
    {
        currentLoadedScene = sceneToLoad;       // ���µ�ǰ����
        playerTrans.position = positionToGo;    // ����player����

        playerTrans.gameObject.SetActive(true);     // �����
        if (fadeSceen)
        {
            //TODO�����뽥��
            fadeEvent.FadeIn(fadeDurationTime);
        }

        isLoading = false;

        if (currentLoadedScene.scenetype == Scenetype.Location)
        {
            // ����������ɺ�ִ�е��¼�
            afterSceneLoadedEvent.RasieEvent();
        }

    }
}
