public class SnailPatrolState : BaseState
{
    public override void OnEnter(Enemy enemy)
    {
        // ��ȡ��ǰ����
        currentEnemy = enemy;
        currentEnemy.currentSpeed = currentEnemy.normalSpeed;
    }

    public override void LogicUpdate()
    {
        //����player�л���chase״̬
        if (currentEnemy.FoundPlayer())
        {
            currentEnemy.SwitchState(NPCState.Skill);       // �л���׷��״̬
        }

        // ����Ƿ�����ǽ����ߵ������±ߣ���֮��ת
        /*�ж�������Ϊ�˷�ֹ��ɫת��֮��β���ٴμ�⵽ǽ�壬�Ӷ��������μ��ת��*/
        if (!currentEnemy.physicsCheck.isGround
            || (currentEnemy.physicsCheck.isTouchLeftWall && currentEnemy.faceDir.x < 0)
            || (currentEnemy.physicsCheck.isTouchRightWall && currentEnemy.faceDir.x > 0))
        {
            currentEnemy.wait = true;
            currentEnemy.animator.SetBool("walk", false);        // ����idle״̬
        }
        else
        {
            currentEnemy.wait = false;
            currentEnemy.animator.SetBool("walk", true);        // ����walk״̬ 
        }
    }

    public override void PhysicsUpdate()
    {

    }

    public override void OnExit()
    {
        currentEnemy.animator.SetBool("walk", false);
    }
}
