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

    public static DataManager instance;     // ����ģʽ
    private List<ISaveable> saveableList = new List<ISaveable>();       // �б�

    private Data saveData;
    public GameObject chest;        // TDOO���ڶ������ı���01��������Ҫ�����������ͱ����

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
        //// ����L��ʵ�ּ�������
        //if (Keyboard.current.lKey.wasPressedThisFrame)
        //{
        //    Load();
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
        if(isdone) {
            chest.SetActive(true);
        }
        else {
            chest.SetActive(false);
        }
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
    }
}
