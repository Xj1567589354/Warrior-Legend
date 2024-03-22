using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Eye : Enemy
{
    [Header("�ƶ���Χ")]
    public float patrolRadius;

    protected override void Awake()
    {
        base.Awake();
        patrolState = new EyePatrolState();
        chaseState = new EyeChaseState();
    }


    /// <summary>
    /// ���ֵ���
    /// </summary>
    /// <returns></returns>
    public override bool FoundPlayer()
    {
        var obj = Physics2D.OverlapCircle(transform.position, checkDistance, attackLayer);
        if (obj)
            attacker = obj.transform;
        return obj;
    }


    /// <summary>
    /// ���Ƽ������
    /// </summary>

    public override void OnDrawGizmosSelected()
    {
        // �����۷����������
        Gizmos.DrawWireSphere(transform.position, checkDistance);
        Gizmos.color = Color.green;
        // �����۷�����ƶ�����
        Gizmos.DrawWireSphere(spwanPoint, patrolRadius);
    }

    /// <summary>
    /// ����õ�Ѳ�ߵ�
    /// </summary>
    /// <returns></returns>
    public override Vector3 GetNewPoint()
    {
        var targetX = Random.Range(-patrolRadius, patrolRadius);
        var targetY = Random.Range(-patrolRadius, patrolRadius);

        return spwanPoint + new Vector3(targetX, targetY);
    }

    /// <summary>
    /// �۷��ƶ�
    /// </summary>
    /*��Enemy�е�move����Ϊ�麯������Ϊ�˲����۷�̳����move����Ҫ��д*/
    public override void Move()
    { }
}
