using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MushChaseState : BaseState
{
    private Attack attack;
    private Vector3 targetPos;      // 目标位置
    private Vector3 moveDir;        // 移动方向
    private bool isAttack;          // 攻击状态
    private float attackRateCounter = 0;    // 攻击计时器

    public override void OnEnter(Enemy enemy)
    {
        currentEnemy = enemy;
        currentEnemy.currentSpeed = currentEnemy.chaseSpeed;
        currentEnemy.animator.SetBool("run", true);
        attack = currentEnemy.GetComponent<Attack>();
    }

    public override void LogicUpdate()
    {
        // 如果丢失计时器小于零，则切换到巡逻状态
        if (currentEnemy.lostTimeCounter <= 0)
            currentEnemy.SwitchState(NPCState.Patrol);

        //// 获取玩家攻击位置
        //targetPos = new Vector3(currentEnemy.attacker.position.x, currentEnemy.attacker.position.y + 1.65f, 0);

        //// 判断攻击距离
        //if (Mathf.Abs(targetPos.x - currentEnemy.transform.position.x) < attack.attackRange
        //    && Mathf.Abs(targetPos.y - currentEnemy.transform.position.y) < attack.attackRange)
        //{
        //    // 攻击
        //    isAttack = true;
        //    if (!currentEnemy.isHurt)
        //        currentEnemy.rb.velocity = Vector2.zero;

        //    // 计时器判断
        //    if (attackRateCounter <= 0)
        //    {
        //        currentEnemy.animator.SetTrigger("attack");
        //        attackRateCounter = attack.attackRate;      // 重置
        //    }
        //}
        //// 超出攻击范围
        //else
        //    isAttack = false;

        // 检测是否碰到墙体或者到达悬崖边，立即翻转
        /*判断条件是为了防止角色转身之后尾部再次检测到墙体，从而导致两次检测转身*/
        if (!currentEnemy.physicsCheck.isGround
             || (currentEnemy.physicsCheck.isTouchLeftWall && currentEnemy.faceDir.x < 0)
             || (currentEnemy.physicsCheck.isTouchRightWall && currentEnemy.faceDir.x > 0))
        {
            currentEnemy.transform.localScale = new Vector3(currentEnemy.faceDir.x, 1, 1);
        }
    }

    public override void PhysicsUpdate()
    {
        // 移动
        if (!currentEnemy.isHurt && !currentEnemy.isDead && !isAttack)
        {
            currentEnemy.rb.velocity = new Vector2(-currentEnemy.faceDir.x * currentEnemy.currentSpeed * Time.deltaTime, currentEnemy.rb.velocity.y);
            Console.Write("good");
        }
    }

    public override void OnExit()
    {
        currentEnemy.animator.SetBool("run", false);
    }
}
