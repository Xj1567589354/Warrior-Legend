using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : MonoBehaviour
{
    [Header("��������")]
    public int damage;      // �˺�ֵ
    public float attackRange;     // ������Χ
    public int attackRate;      // ����Ƶ��

    private void OnTriggerStay2D(Collider2D other)
    {
        /*?--�൱��!null��Ҳ���ǲ�Ϊ���ж�*/
        other.GetComponent<Charactor>()?.TakeDamage(this);
    }
}
