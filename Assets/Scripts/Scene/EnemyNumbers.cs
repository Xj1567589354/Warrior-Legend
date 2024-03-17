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

    private void OnEnable()
    {
        enemyFa = this.transform;
        UpdataChildList();
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
        if (enemyList.Count <= 0 && !istelepoint)
        {
            istelepoint = true;
            telePoint.SetActive(true);
            StartCoroutine(SetCBUI());      // 3s关闭UI
        }
    }

    IEnumerator SetCBUI()
    {
        CenterBottomUI.SetActive(true);
        yield return new WaitForSeconds(3f);
        CenterBottomUI.SetActive(false);
    }
}
