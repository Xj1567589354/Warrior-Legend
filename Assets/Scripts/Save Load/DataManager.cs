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
    public ForeastTPSetSO foreastTPSet;

    public GameSceneSO Foreast;
    public GameSceneSO Cave;
    public GameSceneSO Houce;
    public GameSceneSO Hive;
    public GameSceneSO Boss;

    public static DataManager instance;     // ����ģʽ
    private List<ISaveable> saveableList = new List<ISaveable>();       // �б�
    public SceneLoader sceneLoader;

    private Data saveData;
    public int count;
    public SavePoint forestSP;
    public SavePoint caveSP;

    [Header("Cave�����ʲ�")]
    public GameObject c_Chest;        // TDOO���ڶ������ı���01��������Ҫ�����������ͱ����
    public GameObject c_Chest02;        
    public GameObject c_Chest03;        
    public GameObject caveSavePoint01;
    public GameObject caveSavePoint02;
    public GameObject caveSavePoint03;
    public GameObject c_enemy;
    public GameObject c_prompt;

    [Header("Foreast�����ʲ�")]
    public GameObject f_Chest;
    public GameObject f_Chest02;
    public GameObject forestSavePoint;
    public GameObject telepoint;
    public GameObject f_prompt;
    public GameObject f_enemy;

    [Header("Houce�����ʲ�")]
    public GameObject h_Chest;
    public GameObject houceSavePoint;

    [Header("Hive�����ʲ�")]
    public GameObject hive_Chest;
    public GameObject hive_Chest02;
    public GameObject hive_Chest03;
    public GameObject hive_Chest04;
    public GameObject hive_Chest05;
    public GameObject hive_Chest06;
    public GameObject hiveSavePoint;
    public GameObject hiveSavePoint02;
    public GameObject h_enemy;
    public GameObject h_prompt;

    [Header("Boss�����ʲ�")]
    public GameObject b_enemy;

    [Header("״̬")]
    public bool isPointDone;
    public bool isBoss;         // �Ƿ���Boss�ؿ�

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
                f_enemy.SetActive(true);
                f_prompt.SetActive(true);
            }
            if (currentGameScene == Cave)
            {
                c_Chest.SetActive(true);
                c_Chest02.SetActive(true);
                c_Chest03.SetActive(true);
                caveSavePoint01.SetActive(true);
                caveSavePoint02.SetActive(true);
                caveSavePoint03.SetActive(true);
                c_enemy.SetActive(true);
                c_prompt.SetActive(true);
            }
            if (currentGameScene == Houce)
            {
                h_Chest.SetActive(true);
                houceSavePoint.SetActive(true);
            }
            if (currentGameScene == Hive)
            {
                hive_Chest.SetActive(true);
                hive_Chest02.SetActive(true);
                hive_Chest03.SetActive(true);
                hive_Chest04.SetActive(true);
                hive_Chest05.SetActive(true);
                hive_Chest06.SetActive(true);
                hiveSavePoint.SetActive(true);
                hiveSavePoint02.SetActive(true);
                h_prompt.SetActive(true);
                h_enemy.SetActive(true);
                isBoss = true;
            }
            if (currentGameScene == Boss)
            {
                b_enemy.SetActive(true);
            }
        }
        else
        {
            if (currentGameScene == Foreast)
            {
                f_Chest.SetActive(false);
                f_Chest02.SetActive(false);
                forestSavePoint.SetActive(false);
                f_enemy.SetActive(false);
                f_prompt.SetActive(false);
            }
            if (currentGameScene == Cave)
            {
                c_Chest.SetActive(false);
                c_Chest02.SetActive(false);
                c_Chest03.SetActive(false);
                caveSavePoint01.SetActive(false);
                caveSavePoint02.SetActive(false);
                caveSavePoint03.SetActive(false);
                c_enemy.SetActive(false);
                c_prompt.SetActive(false);
            }
            if (currentGameScene == Houce)
            {
                h_Chest.SetActive(false);
                houceSavePoint.SetActive(false);
            }
            if (currentGameScene == Hive)
            {
                hive_Chest.SetActive(false);
                hive_Chest02.SetActive(false);
                hive_Chest03.SetActive(false);
                hive_Chest04.SetActive(false);
                hive_Chest05.SetActive(false);
                hive_Chest06.SetActive(false);
                hiveSavePoint.SetActive(false);
                hiveSavePoint02.SetActive(false);
                h_prompt.SetActive(false);
                h_enemy.SetActive(false);
            }
            if (currentGameScene == Boss)
            {
                b_enemy.SetActive(false);
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
        foreastTPSet.onEventRaised += SetForeastTP;
    }

    private void OnDisable()
    {
        SaveDataEvent.onEventRaised -= Save;
        SavePersistEvent.onEventRaised -= SetAssetsActive;
        loadDataEvent.onEventRaised -= Load;
        foreastTPSet.onEventRaised -= SetForeastTP;
    }

    /// <summary>
    /// ����Foreast������TelePoint2
    /// </summary>
    /// <exception cref="NotImplementedException"></exception>
    private void SetForeastTP()
    {
        telepoint.SetActive(true);

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
