using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Permissions;
using UnityEngine;
using UnityEngine.InputSystem;

public class Chest : MonoBehaviour, Iinteractable, ISaveable
{
    public Sprite openSprite;
    public Sprite closeSprite;
    private SpriteRenderer spriteRenderer;
    public bool isDone;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void OnEnable()
    {
        spriteRenderer.sprite = isDone ? openSprite : closeSprite;      // 根据isDone状态来更换chest图片

        ISaveable saveable = this;
        saveable.RegisterSaveData();
    }

    private void OnDisable()
    {
        ISaveable saveable = this;
        saveable.UnRegisterSaveData();
    }

    public void TriggerAction()
    {
        //Debug.Log("Open Chest");
        if(!isDone) {
            OpenChest();
        }
    }

    // 打开宝箱
    private void OpenChest()
    {
        spriteRenderer.sprite = openSprite;
        isDone = true;

        this.gameObject.tag = "Untagged";       // 关闭互动标识
    }

    public DataDefinition GetDataID()
    {
        return GetComponent<DataDefinition>();
    }

    public void GetSaveData(Data data)
    {
        if (data.boolSaveDataDict.ContainsKey(GetDataID().ID))
        {
            data.boolSaveDataDict[GetDataID().ID + "Chest01"] = isDone;
        }
        else
        {
            data.boolSaveDataDict.Add(GetDataID().ID+ "Chest01", isDone);
        }
    }

    public void LoadData(Data data)
    {
        isDone = data.boolSaveDataDict[GetDataID().ID + "Chest01"];
    }
}
