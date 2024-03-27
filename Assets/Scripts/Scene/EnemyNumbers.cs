using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyNumbers : MonoBehaviour
{
    public List<GameObject> enemyList = new List<GameObject>();        // List�洢���е���
    private Transform enemyFa;      // ���е��˸�����
    public GameObject telePoint;    // ���͵�

    public GameObject CenterBottomUI;
    public bool istelepoint;
    public bool isBoss;         // �Ƿ���boss�ؿ�

    public FadeEventSO fadeEvent;
    public FadeEventSO textFadeEvent;

    private Color color;
    private Color textColor;

    private void OnEnable()
    {
        enemyFa = this.transform;
        UpdataChildList();

        if (!isBoss)
        {
            color = new Color(0, 0, 0, 0.6f);
            textColor = new Color(0, 1, 0.7f, 1);
        }
    }


    public void UpdataChildList()
    {
        enemyList.Clear();      // ����б�

        int childCount = transform.childCount;      // ��ȡ������������Ӷ�������
        // �������Ӷ������list��
        for (int i = 0; i < childCount; i++)
        {
            Transform childTransform = transform.GetChild(i);
            enemyList.Add(childTransform.gameObject);
        }
    }

    private void Update()
    {
        if (enemyList.Count <= 0 && !istelepoint && !isBoss)
        {
            istelepoint = true;
            telePoint.SetActive(true);
            
            // ��������ʾ
            fadeEvent.FadeIn(color, 1.5f, "YesT");
            textFadeEvent.FadeIn(textColor, 1.5f, "YesT");
            Invoke("SetCBUI", 3f);
        }
    }

    /// <summary>
    ///�رմ�������ʾ
    /// </summary>
    void SetCBUI()
    {
        // ��������ʾ
        fadeEvent.FadeOut(1.5f, "YesT");
        textFadeEvent.FadeOut(1.5f, "YesT");
    }
}
