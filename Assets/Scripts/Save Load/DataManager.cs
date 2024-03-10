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

    public GameSceneSO Foreast;
    public GameSceneSO Cave;

    public static DataManager instance;     // 单例模式
    private List<ISaveable> saveableList = new List<ISaveable>();       // 列表
    public SceneLoader sceneLoader;

    private Data saveData;
    public int count;

    [Header("Cave场景资产")]
    public GameObject c_Chest;        // TDOO：第二场景的宝箱01，后序还需要添加其他宝箱和保存点
    public GameObject c_Chest02;        
    public GameObject c_Chest03;        
    public GameObject caveSavePoint01;
    public GameObject caveSavePoint02;
    public GameObject caveSavePoint03;

    [Header("Foreast场景资产")]
    public GameObject f_Chest;
    public GameObject f_Chest02;
    public GameObject forestSavePoint;

    public SavePoint forestSP;
    public SavePoint caveSP;

    [Header("状态")]
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

        // 保证保存点开启时，在此保存点前面的保存点也是开启状态
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
