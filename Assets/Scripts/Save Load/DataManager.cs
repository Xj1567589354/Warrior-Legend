using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[DefaultExecutionOrder(order:-100)]
public class DataManager : MonoBehaviour
{
    [Header("监听")]
    public VoidEventSO SaveDataEvent;

    public static DataManager instance;     // 单例模式
    private List<ISaveable> saveableList = new List<ISaveable>();       // 列表

    private Data saveData;

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
        // 按下L键实现加载数据
        if (Keyboard.current.lKey.wasPressedThisFrame)
        {
            Load();
        }
    }

    private void OnEnable()
    {
        SaveDataEvent.onEventRaised += Save;
    }

    private void OnDisable()
    {
        SaveDataEvent.onEventRaised -= Save;
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

        //foreach (var item in saveData.characterPosDict)
        //{
        //    Debug.Log(item.Key + "   "+ item.Value);
        //}
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
