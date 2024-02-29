using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SavePoint : MonoBehaviour, Iinteractable
{
    [Header("�㲥")]
    public VoidEventSO LoadGameEvent;

    [Header("��������")]
    public SpriteRenderer spriteRenderer;
    public GameObject lightObj;

    // ��������ͼƬ
    public Sprite darkSprite;
    public Sprite lightSprite;

    public bool isDone;

    private void OnEnable()
    {
        spriteRenderer.sprite = isDone ? lightSprite : darkSprite;
        lightObj.SetActive(isDone);
    }

    public void TriggerAction()
    {
        if (!isDone)
        {
            isDone = true;
            spriteRenderer.sprite = lightSprite;
            lightObj.SetActive(true);

            // ���б�������
            LoadGameEvent.RasieEvent();     

            this.gameObject.tag = "Untagged";       // ȡ�����λ���
        }
    }
}
