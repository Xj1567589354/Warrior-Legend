using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyNumbers : MonoBehaviour
{
    public List<GameObject> enemyList = new List<GameObject>();        // List存储所有敌人
    private Transform enemyFa;      // 所有敌人父对象
    public GameObject telePoint;    // 传送点

    public GameObject CenterBottomUI;
    public bool istelepoint;
    public bool isBoss;         // 是否是boss关卡

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
        enemyList.Clear();      // 清空列表

        int childCount = transform.childCount;      // 获取父对象的所有子对象数量
        // 将所有子对象存入list中
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
            
            // 传送门提示
            fadeEvent.FadeIn(color, 1.5f, "YesT");
            textFadeEvent.FadeIn(textColor, 1.5f, "YesT");
            Invoke("SetCBUI", 3f);
        }
    }

    /// <summary>
    ///关闭传送门提示
    /// </summary>
    void SetCBUI()
    {
        // 传送门提示
        fadeEvent.FadeOut(1.5f, "YesT");
        textFadeEvent.FadeOut(1.5f, "YesT");
    }
}
