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
        spriteRenderer.sprite = isDone ? openSprite : closeSprite;      // ����isDone״̬������chestͼƬ
    }

    public void TriggerAction()
    {
        //Debug.Log("Open Chest");
        if(!isDone) {
            OpenChest();
        }
    }

    // �򿪱���
    private void OpenChest()
    {
        spriteRenderer.sprite = openSprite;
        isDone = true;

        this.gameObject.tag = "Untagged";       // �رջ�����ʶ
    }
}
