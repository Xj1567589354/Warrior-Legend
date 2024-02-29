using System.Collections;
using System.Collections.Generic;
using System.Security.Permissions;
using UnityEngine;

public class Chest : MonoBehaviour, Iinteractable
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
}
