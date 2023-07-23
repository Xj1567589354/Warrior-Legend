using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skeleton : Enemy
{
    protected override void Awake()
    {
        base.Awake();
        patrolState = new SkeletonPatrolState();
        chaseState = new SkeletonChaseState();
    }

    public override void Move()
    {

    }

    public override void OnTakeDamage(Transform attackerTrans)
    {
        // 玩家
        attacker = attackerTrans;

        // 转身
        if (attacker.position.x - transform.position.x > 0)
            transform.localScale = new Vector3(1, 1, 1);
        if (attacker.position.x - transform.position.x < 0)
            transform.localScale = new Vector3(-1, 1, 1);

        // 受伤击退
        isHurt = true;
        animator.SetTrigger("hurt");
        hurtDir = new Vector2(transform.position.x - attacker.position.x, 0).normalized;
        /*野猪受伤时将x轴上的力停下来，防止野猪原地受伤动画*/
        rb.velocity = Vector2.zero;
        StartCoroutine(OnHurt(hurtDir));
    }
}
