using UnityEngine;

public class BeePatrolState : BaseState
{
    private Vector3 targetPos;      // 移动目标位置
    private Vector3 moveDir;        // 移动方向
    public override void OnEnter(Enemy enemy)
    {
        currentEnemy = enemy;
        currentEnemy.currentSpeed = currentEnemy.normalSpeed;
        targetPos = currentEnemy.GetNewPoint();     // 得到新的移动目标点
    }
    public override void LogicUpdate()
    {
        // 是否发现敌人
        if (currentEnemy.FoundPlayer())
            currentEnemy.SwitchState(NPCState.Chase);

        // 是否到达移动目标点
        if(Mathf.Abs(targetPos.x - currentEnemy.transform.position.x) < 0.1f
            && Mathf.Abs(targetPos.y - currentEnemy.transform.position.y) < 0.1f)
        {
            currentEnemy.wait = true;
            targetPos = currentEnemy.GetNewPoint();     // 得到新的移动目标点
        }

        // 获得移动方向
        moveDir = (targetPos - currentEnemy.transform.position).normalized;

        // 改变面朝方向
        if (moveDir.x > 0)
            currentEnemy.transform.localScale = new Vector3(-1, 1, 1);
        if (moveDir.x < 0)
            currentEnemy.transform.localScale = new Vector3(1, 1, 1);
    }

    public override void PhysicsUpdate()
    {
        // 移动
        if (!currentEnemy.wait && !currentEnemy.isHurt && !currentEnemy.isDead)
            currentEnemy.rb.velocity = moveDir * currentEnemy.currentSpeed * Time.deltaTime;
        else
            currentEnemy.rb.velocity = Vector2.zero;
    }
    public override void OnExit()
    {

    }
}
