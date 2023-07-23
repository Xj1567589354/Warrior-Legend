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
        // ���
        attacker = attackerTrans;

        // ת��
        if (attacker.position.x - transform.position.x > 0)
            transform.localScale = new Vector3(1, 1, 1);
        if (attacker.position.x - transform.position.x < 0)
            transform.localScale = new Vector3(-1, 1, 1);

        // ���˻���
        isHurt = true;
        animator.SetTrigger("hurt");
        hurtDir = new Vector2(transform.position.x - attacker.position.x, 0).normalized;
        /*Ұ������ʱ��x���ϵ���ͣ��������ֹҰ��ԭ�����˶���*/
        rb.velocity = Vector2.zero;
        StartCoroutine(OnHurt(hurtDir));
    }
}
