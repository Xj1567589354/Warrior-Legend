using System.Collections;
using System.Collections.Generic;
using System.Security.Permissions;
using UnityEngine;
using UnityEngine.InputSystem;

public class Chest : MonoBehaviour, Iinteractable, ISaveable
{
    public Sprite openSprite;
    public Sprite closeSprite;
    public bool isDone;
    private SpriteRenderer spriteRenderer;

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

    private void Update()
    {
        //Debug.Log("Open Chest");
        if (isDone)
        {
            spriteRenderer.sprite = openSprite;
            this.gameObject.tag = "Untagged";       // 关闭互动标识
        }
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
        if(data.boolSaveDataDict.ContainsKey(GetDataID().ID))
        {
            data.boolSaveDataDict[GetDataID().ID] = this.isDone;
            print("s");
        }
        else
        {
            data.boolSaveDataDict.Add(GetDataID().ID, this.isDone);
            print("s");
        }
    }

    public void LoadData(Data data)
    {
        if (data.boolSaveDataDict.ContainsKey(GetDataID().ID))
        {
            this.isDone = data.boolSaveDataDict[GetDataID().ID];
            print(isDone);
        }
    }
}
