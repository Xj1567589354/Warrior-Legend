using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
    public int count;

    [Header("Cave�����ʲ�")]
    public GameObject c_Chest;        // TDOO���ڶ������ı���01��������Ҫ�����������ͱ����
    public GameObject c_Chest02;        
    public GameObject c_Chest03;        
    public GameObject caveSavePoint01;
    public GameObject caveSavePoint02;
    public GameObject caveSavePoint03;

    [Header("Foreast�����ʲ�")]
    public GameObject f_Chest;
    public GameObject f_Chest02;
    public GameObject forestSavePoint;

    public SavePoint forestSP;
    public SavePoint caveSP;

    [Header("״̬")]
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

        if (isPointDone)
        {
            if (currentGameScene == Foreast)
            {
                f_Chest.SetActive(true);
                f_Chest02.SetActive(true);
                forestSavePoint.SetActive(true);
                caveSavePoint01.SetActive(false);
                caveSavePoint02.SetActive(false);
                caveSavePoint03.SetActive(false);
            }
            if (currentGameScene == Cave)
            {
                c_Chest.SetActive(true);
                c_Chest02.SetActive(true);
                c_Chest03.SetActive(true);
                caveSavePoint01.SetActive(true);
                caveSavePoint02.SetActive(true);
                caveSavePoint03.SetActive(true);
                forestSavePoint.SetActive(false);
            }
        }
        else
        {
            if (currentGameScene == Foreast)
            {
                f_Chest.SetActive(false);
                f_Chest02.SetActive(false);
                forestSavePoint.SetActive(false);
            }
            if (currentGameScene == Cave)
            {
                c_Chest.SetActive(false);
                c_Chest02.SetActive(false);
                c_Chest03.SetActive(false);
                caveSavePoint01.SetActive(false);
                caveSavePoint02.SetActive(false);
                caveSavePoint03.SetActive(false);
            }
        }

        // ��֤����㿪��ʱ���ڴ˱����ǰ��ı����Ҳ�ǿ���״̬
        if (caveSP.isDone)
            forestSP.isDone = true;
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
    private void SetAssetsActive(bool isdone, GameSceneSO gameScene)
    {
        isPointDone = isdone;
        currentGameScene = gameScene;
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
    }
}
