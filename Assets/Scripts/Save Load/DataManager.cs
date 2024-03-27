using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

[DefaultExecutionOrder(order:-100)]
public class DataManager : MonoBehaviour
{
    [Header("监听")]
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

    public static DataManager instance;     // 单例模式
    private List<ISaveable> saveableList = new List<ISaveable>();       // 列表
    public SceneLoader sceneLoader;

    private Data saveData;
    public int count;
    public SavePoint forestSP;
    public SavePoint caveSP;

    [Header("Cave场景资产")]
    public GameObject c_Chest;        // TDOO：第二场景的宝箱01，后序还需要添加其他宝箱和保存点
    public GameObject c_Chest02;        
    public GameObject c_Chest03;        
    public GameObject caveSavePoint01;
    public GameObject caveSavePoint02;
    public GameObject caveSavePoint03;
    public GameObject c_enemy;
    public GameObject c_prompt;

    [Header("Foreast场景资产")]
    public GameObject f_Chest;
    public GameObject f_Chest02;
    public GameObject forestSavePoint;
    public GameObject telepoint;
    public GameObject f_prompt;
    public GameObject f_enemy;

    [Header("Houce场景资产")]
    public GameObject h_Chest;
    public GameObject houceSavePoint;

    [Header("Hive场景资产")]
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

    [Header("Boss场景资产")]
    public GameObject b_enemy;

    [Header("状态")]
    public bool isPointDone;
    public bool isBoss;         // 是否开启Boss关卡

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

        // 保证保存点开启时，在此保存点前面的保存点也是开启状态
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
    /// 激活Foreast场景的TelePoint2
    /// </summary>
    /// <exception cref="NotImplementedException"></exception>
    private void SetForeastTP()
    {
        telepoint.SetActive(true);

    }

    /// <summary>
    /// 激活宝箱和保存点
    /// </summary>
    /// <param name="isdone">是否激活</param>
    private void SetAssetsActive(bool isdone, GameSceneSO gameScene)
    {
        isPointDone = isdone;
        currentGameScene = gameScene;
    }

    /// <summary>
    /// 保存数据注册
    /// </summary>
    /// <param name="saveable"></param>
    public void RegisterSaveData(ISaveable saveable)
    {
        // 判断列表当中是否包含了saveable资产
        if (!saveableList.Contains(saveable))
        {
            saveableList.Add(saveable);
        }
    }

    /// <summary>
    /// 保存数据注销
    /// </summary>
    /// <param name="saveable"></param>
    public void UnRegisterSaveData(ISaveable saveable)
    {
        saveableList.Remove(saveable);
    }

    /// <summary>
    /// 保存数据
    /// </summary>
    public void Save()
    {
        // 循环遍历list当中的每一个saveable资产，将各自的坐标传入saveData当中的
        foreach (var saveable in saveableList)
        {
            saveable.GetSaveData(saveData);     // 保存每一个saveable资产的坐标数据
        }
    }

    /// <summary>
    ///  加载数据
    /// </summary>
    public void Load()
    {
        foreach(var saveable in saveableList)
        {
            saveable.LoadData(saveData);        // 加载每一个saveable资产的坐标数据
        }
    }
}
