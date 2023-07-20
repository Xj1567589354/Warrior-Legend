public class SnailPatrolState : BaseState
{
    public override void OnEnter(Enemy enemy)
    {
        // 获取当前敌人
        currentEnemy = enemy;
        currentEnemy.currentSpeed = currentEnemy.normalSpeed;
    }

    public override void LogicUpdate()
    {
        //发现player切换到chase状态
        if (currentEnemy.FoundPlayer())
        {
            currentEnemy.SwitchState(NPCState.Skill);       // 切换到追击状态
        }

        // 检测是否碰到墙体或者到达悬崖边，随之翻转
        /*判断条件是为了防止角色转身之后尾部再次检测到墙体，从而导致两次检测转身*/
        if (!currentEnemy.physicsCheck.isGround
            || (currentEnemy.physicsCheck.isTouchLeftWall && currentEnemy.faceDir.x < 0)
            || (currentEnemy.physicsCheck.isTouchRightWall && currentEnemy.faceDir.x > 0))
        {
            currentEnemy.wait = true;
            currentEnemy.animator.SetBool("walk", false);        // 进入idle状态
        }
        else
        {
            currentEnemy.wait = false;
            currentEnemy.animator.SetBool("walk", true);        // 进入walk状态 
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
