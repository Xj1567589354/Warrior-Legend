using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SavePoint : MonoBehaviour, Iinteractable, ISaveable
{
    [Header("广播")]
    public VoidEventSO SaveDataEvent;

    [Header("变量参数")]
    public SpriteRenderer spriteRenderer;
    public GameObject lightObj;

    // 两种文字图片
    public Sprite darkSprite;
    public Sprite lightSprite;

    public bool isDone;

    private void OnEnable()
    {
        spriteRenderer.sprite = isDone ? lightSprite : darkSprite;
        this.gameObject.tag = isDone ? "Untagged" : "Interactable";
        if (DataManager.instance.currentGameScene == DataManager.instance.Cave)
            lightObj.SetActive(isDone);

        ISaveable saveable = this;
        saveable.RegisterSaveData();
    }

    private void Update()
    {
        spriteRenderer.sprite = isDone ? lightSprite : darkSprite;
        this.gameObject.tag = isDone ? "Untagged" : "Interactable";
        if (DataManager.instance.currentGameScene == DataManager.instance.Cave)
            lightObj.SetActive(isDone);
    }

    private void OnDisable()
    {
        ISaveable saveable = this;
        saveable.UnRegisterSaveData();
    }

    public void TriggerAction()
    {
        if (!isDone)
        {
            isDone = true;
            spriteRenderer.sprite = lightSprite;
            if (DataManager.instance.currentGameScene == DataManager.instance.Cave)
                lightObj.SetActive(isDone);

            // 呼叫保存数据
            SaveDataEvent.RasieEvent(); 

            this.gameObject.tag = "Untagged";       // 取消二次互动

            DataManager.instance.count++;
        }
    }

    public DataDefinition GetDataID()
    {
        return GetComponent<DataDefinition>();
    }

    public void GetSaveData(Data data)
    {
        if (data.boolSaveDataDict.ContainsKey(GetDataID().ID))
        {
            data.boolSaveDataDict[GetDataID().ID] = isDone;
        }
        else
            data.boolSaveDataDict.Add(GetDataID().ID, isDone);
    }

    public void LoadData(Data data)
    {
        if (data.boolSaveDataDict.ContainsKey(GetDataID().ID))
            this.isDone = data.boolSaveDataDict[GetDataID().ID];
        print("LoadPoint");
    }
}
