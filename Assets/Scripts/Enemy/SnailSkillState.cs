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
        /*如果一直有丢失时间，就代表发现了敌人，就一直处于无敌状态*/
        currentEnemy.GetComponent<Charactor>().invulnerableCounter = currentEnemy.lostTimeCounter;
    }

    public override void LogicUpdate()
    {
        // 如果丢失计时器小于零，则切换到巡逻状态
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
