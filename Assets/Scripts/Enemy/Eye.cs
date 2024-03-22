using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Eye : Enemy
{
    [Header("移动范围")]
    public float patrolRadius;

    protected override void Awake()
    {
        base.Awake();
        patrolState = new EyePatrolState();
        chaseState = new EyeChaseState();
    }


    /// <summary>
    /// 发现敌人
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
    /// 绘制检测区域
    /// </summary>

    public override void OnDrawGizmosSelected()
    {
        // 绘制蜜蜂检测敌人区域
        Gizmos.DrawWireSphere(transform.position, checkDistance);
        Gizmos.color = Color.green;
        // 绘制蜜蜂随机移动区域
        Gizmos.DrawWireSphere(spwanPoint, patrolRadius);
    }

    /// <summary>
    /// 随机得到巡逻点
    /// </summary>
    /// <returns></returns>
    public override Vector3 GetNewPoint()
    {
        var targetX = Random.Range(-patrolRadius, patrolRadius);
        var targetY = Random.Range(-patrolRadius, patrolRadius);

        return spwanPoint + new Vector3(targetX, targetY);
    }

    /// <summary>
    /// 蜜蜂移动
    /// </summary>
    /*将Enemy中的move设置为虚函数，是为了不让蜜蜂继承这个move，需要重写*/
    public override void Move()
    { }
}
