using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EyeChaseState : BaseState
{
    private Attack attack;
    private Vector3 targetPos;      // Ŀ��λ��
    private Vector3 moveDir;        // �ƶ�����
    private bool isAttack;          // ����״̬
    private float attackRateCounter = 0;    // ������ʱ��
    public override void OnEnter(Enemy enemy)
    {
        currentEnemy = enemy;
        currentEnemy.currentSpeed = currentEnemy.chaseSpeed;
        attack = currentEnemy.GetComponent<Attack>();

        currentEnemy.lostTimeCounter = currentEnemy.lostTime;
        currentEnemy.animator.SetBool("chase", true);
    }

    public override void LogicUpdate()
    {
        // �����ʧ��ʱ��С���㣬���л���Ѳ��״̬
        if (currentEnemy.lostTimeCounter <= 0)
            currentEnemy.SwitchState(NPCState.Patrol);

        // ��ʱ��
        attackRateCounter -= Time.deltaTime;

        // ��ȡ��ҹ���λ��
        targetPos = new Vector3(currentEnemy.attacker.position.x, currentEnemy.attacker.position.y + 1.65f, 0);

        // �жϹ�������
        if (Mathf.Abs(targetPos.x - currentEnemy.transform.position.x) < attack.attackRange
            && Mathf.Abs(targetPos.y - currentEnemy.transform.position.y) < attack.attackRange)
        {
            // ����
            isAttack = true;
            if (!currentEnemy.isHurt)
                currentEnemy.rb.velocity = Vector2.zero;

            // ��ʱ���ж�
            if (attackRateCounter <= 0)
            {
                currentEnemy.animator.SetTrigger("attack");
                attackRateCounter = attack.attackRate;      // ����
            }
        }
        // ����������Χ
        else
            isAttack = false;

        // ����ƶ�����
        moveDir = (targetPos - currentEnemy.transform.position).normalized;

        // �ı��泯����
        if (moveDir.x > 0)
            currentEnemy.transform.localScale = new Vector3(1, 1, 1);
        if (moveDir.x < 0)
            currentEnemy.transform.localScale = new Vector3(-1, 1, 1);
    }

    public override void PhysicsUpdate()
    {
        // �ƶ�
        if (!currentEnemy.isHurt && !currentEnemy.isDead && !isAttack)
            currentEnemy.rb.velocity = moveDir * currentEnemy.currentSpeed * Time.deltaTime;
    }

    public override void OnExit()
    {
        currentEnemy.animator.SetBool("chase", false);
    }
}
