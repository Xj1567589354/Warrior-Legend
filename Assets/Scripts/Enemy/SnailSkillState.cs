public class SnailSkillState : BaseState
{
    public override void OnEnter(Enemy enemy)
    {
        currentEnemy = enemy;
        currentEnemy.currentSpeed = 0;
        currentEnemy.animator.SetBool("walk", false);
        currentEnemy.animator.SetBool("hide", true);
        currentEnemy.animator.SetTrigger("skill");

        currentEnemy.lostTimeCounter = currentEnemy.lostTime;

        currentEnemy.GetComponent<Charactor>().invulnerable = true;
        /*���һֱ�ж�ʧʱ�䣬�ʹ������˵��ˣ���һֱ�����޵�״̬*/
        currentEnemy.GetComponent<Charactor>().invulnerableCounter = currentEnemy.lostTimeCounter;
    }

    public override void LogicUpdate()
    {
        // �����ʧ��ʱ��С���㣬���л���Ѳ��״̬
        if (currentEnemy.lostTimeCounter <= 0)
            currentEnemy.SwitchState(NPCState.Patrol);
        currentEnemy.GetComponent<Charactor>().invulnerableCounter = currentEnemy.lostTimeCounter;
    }

    public override void PhysicsUpdate()
    {

    }
    public override void OnExit()
    {
        currentEnemy.animator.SetBool("hide", false);
        currentEnemy.GetComponent<Charactor>().invulnerable = false;
    }
}
