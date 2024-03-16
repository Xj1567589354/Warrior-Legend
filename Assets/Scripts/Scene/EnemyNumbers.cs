using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyNumbers : MonoBehaviour
{
    public List<GameObject> enemyList = new List<GameObject>();        // List存储所有敌人
    private Transform enemyFa;      // 所有敌人父对象
    public GameObject telePoint;    // 传送点

    private void Awake()
    {

    }

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
        if (enemyList.Count > 0)
        {
            UpdataChildList();      // 更新列表
        }
        else
            telePoint.tag = "Interactable";
    }
}
