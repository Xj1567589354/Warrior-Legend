using UnityEngine;

public class SkeletonPatrolState : BaseState
{
    public override void OnEnter(Enemy enemy)
    {
        // ��ȡ��ǰ����
        currentEnemy = enemy;
        currentEnemy.currentSpeed = currentEnemy.normalSpeed;
        Debug.Log("Enter Patrol");
    }
    public override void LogicUpdate()
    {
        //����player�л���chase״̬
        if (currentEnemy.FoundPlayer())
            currentEnemy.SwitchState(NPCState.Chase);       // �л���׷��״̬

        // ����Ƿ�����ǽ����ߵ������±ߣ���֮��ת
        /*�ж�������Ϊ�˷�ֹ��ɫת��֮��β���ٴμ�⵽ǽ�壬�Ӷ��������μ��ת��*/
        if (!currentEnemy.physicsCheck.isGround
            || (currentEnemy.physicsCheck.isTouchLeftWall && currentEnemy.faceDir.x > 0)
            || (currentEnemy.physicsCheck.isTouchRightWall && currentEnemy.faceDir.x < 0))
        {
            currentEnemy.wait = true;
            currentEnemy.animator.SetBool("walk", false);        // ����idle״̬
            currentEnemy.lostTimeCounter = 0;
        }
        else
        {
            currentEnemy.wait = false;
            currentEnemy.animator.SetBool("walk", true);        // ����walk״̬ 
        }
    }

    public override void PhysicsUpdate()
    {
        if (!currentEnemy.wait && !currentEnemy.isHurt && !currentEnemy.isDead)
            currentEnemy.rb.velocity = new Vector2(-currentEnemy.faceDir.x * currentEnemy.currentSpeed * Time.deltaTime, currentEnemy.rb.velocity.y);
        else
            currentEnemy.rb.velocity = Vector2.zero;
    }
    public override void OnExit()
    {
        currentEnemy.animator.SetBool("walk", false);
        Debug.Log("Exit Patrol");
    }
}
