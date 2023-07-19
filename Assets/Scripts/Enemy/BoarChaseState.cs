using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoarChaseState : BaseState
{
    public override void OnEnter(Enemy enemy)
    {
        currentEnemy = enemy;
        currentEnemy.currentSpeed = currentEnemy.chaseSpeed;
        currentEnemy.animator.SetBool("run", true);
    }

    public override void LogicUpdate()
    {
        // 如果丢失计时器小于零，则切换到巡逻状态
        if (currentEnemy.lostTimeCounter <= 0)
            currentEnemy.SwitchState(NPCState.Patrol);


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

    }

    public override void OnExit()
    {
        currentEnemy.animator.SetBool("run", false);
    }
}
