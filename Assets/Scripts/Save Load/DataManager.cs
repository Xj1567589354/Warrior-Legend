using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[DefaultExecutionOrder(order:-100)]
public class DataManager : MonoBehaviour
{
    [Header("����")]
    public VoidEventSO SaveDataEvent;
    public PersistEventSO SavePersistEvent;
    public VoidEventSO loadDataEvent;
    public GameSceneSO currentGameScene;

    public GameSceneSO Foreast;
    public GameSceneSO Cave;

    public static DataManager instance;     // ����ģʽ
    private List<ISaveable> saveableList = new List<ISaveable>();       // �б�
    public SceneLoader sceneLoader;

    private Data saveData;
    public GameObject chest;        // TDOO���ڶ������ı���01��������Ҫ�����������ͱ����
    public GameObject caveSavePoint01;
    public GameObject forestSavePoint;

    public bool isPointDone;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
            Destroy(this.gameObject);

        saveData = new Data();
    }

    private void Update()
    {
        //currentGameScene = sceneLoader.currentLoadedScene;

        //if (isPointDone)
        //{
        //    if (currentGameScene == Foreast)
        //    {
        //        forestSavePoint.SetActive(true);
        //        caveSavePoint01.SetActive(false);
        //    }
        //    if (currentGameScene == Cave)
        //    {
        //        chest.SetActive(true);
        //        caveSavePoint01.SetActive(true);
        //        forestSavePoint.SetActive(false);
        //    }
        //}
        //else
        //{
        //    if (currentGameScene == Foreast)
        //    {
        //        forestSavePoint.SetActive(false);
        //    }
        //    if (currentGameScene == Cave)
        //    {
        //        chest.SetActive(false);
        //        caveSavePoint01.SetActive(false);
        //    }
        //}
    }

    private void OnEnable()
    {
        SaveDataEvent.onEventRaised += Save;
        SavePersistEvent.onEventRaised += SetAssetsActive;
        loadDataEvent.onEventRaised += Load;
    }

    private void OnDisable()
    {
        SaveDataEvent.onEventRaised -= Save;
        SavePersistEvent.onEventRaised -= SetAssetsActive;
        loadDataEvent.onEventRaised -= Load;
    }

    /// <summary>
    /// �����ͱ����
    /// </summary>
    /// <param name="isdone">�Ƿ񼤻�</param>
    private void SetAssetsActive(bool isdone)
    {
        isPointDone = isdone;
    }

    /// <summary>
    /// ��������ע��
    /// </summary>
    /// <param name="saveable"></param>
    public void RegisterSaveData(ISaveable saveable)
    {
        // �ж��б����Ƿ������saveable�ʲ�
        if (!saveableList.Contains(saveable))
        {
            saveableList.Add(saveable);
        }
    }

    /// <summary>
    /// ��������ע��
    /// </summary>
    /// <param name="saveable"></param>
    public void UnRegisterSaveData(ISaveable saveable)
    {
        saveableList.Remove(saveable);
    }

    /// <summary>
    /// ��������
    /// </summary>
    public void Save()
    {
        // ѭ������list���е�ÿһ��saveable�ʲ��������Ե����괫��saveData���е�
        foreach (var saveable in saveableList)
        {
            saveable.GetSaveData(saveData);     // ����ÿһ��saveable�ʲ�����������
        }

        //foreach (var item in saveData.characterPosDict)
        //{
        //    Debug.Log(item.Key + "   "+ item.Value);
        //}
    }

    /// <summary>
    ///  ��������
    /// </summary>
    public void Load()
    {
        foreach(var saveable in saveableList)
        {
            saveable.LoadData(saveData);        // ����ÿһ��saveable�ʲ�����������
        }

        currentGameScene = sceneLoader.currentLoadedScene;
    }
}
