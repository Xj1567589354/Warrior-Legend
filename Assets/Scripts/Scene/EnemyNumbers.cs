using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyNumbers : MonoBehaviour
{
    public List<GameObject> enemyList = new List<GameObject>();        // List�洢���е���
    private Transform enemyFa;      // ���е��˸�����
    public GameObject telePoint;    // ���͵�

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
        if (enemyList.Count > 0)
        {
            UpdataChildList();      // �����б�
        }
        else
            telePoint.tag = "Interactable";
    }
}
