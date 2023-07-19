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
        // �����ʧ��ʱ��С���㣬���л���Ѳ��״̬
        if (currentEnemy.lostTimeCounter <= 0)
            currentEnemy.SwitchState(NPCState.Patrol);


        // ����Ƿ�����ǽ����ߵ������±ߣ�������ת
        /*�ж�������Ϊ�˷�ֹ��ɫת��֮��β���ٴμ�⵽ǽ�壬�Ӷ��������μ��ת��*/
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
