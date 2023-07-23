using UnityEngine;

public class SkeletonChaseState : BaseState
{
    private Attack attack;
    private Vector3 targetPos;      // 目标位置
    private bool isAttack;
    private float attackRateCounter;    // 攻击计时器
    public override void OnEnter(Enemy enemy)
    {
        currentEnemy = enemy;
        currentEnemy.currentSpeed = currentEnemy.chaseSpeed;
        currentEnemy.animator.SetBool("run", true);
        attack = currentEnemy.GetComponent<Attack>();
        currentEnemy.lostTimeCounter = currentEnemy.lostTime;
        Debug.Log("Enter Chase");
    }
    public override void LogicUpdate()
    {
        // 如果丢失计时器小于零，则切换到巡逻状态
        if (currentEnemy.lostTimeCounter <= 0)
            currentEnemy.SwitchState(NPCState.Patrol);

        if (!currentEnemy.physicsCheck.isGround
            || (currentEnemy.physicsCheck.isTouchLeftWall && currentEnemy.faceDir.x > 0)
            || (currentEnemy.physicsCheck.isTouchRightWall && currentEnemy.faceDir.x < 0))
            {
                currentEnemy.transform.localScale = new Vector3(-currentEnemy.faceDir.x, 1, 1);
                currentEnemy.lostTimeCounter = 0;
            }

        // 计时器
        attackRateCounter -= Time.deltaTime;

        // 获取玩家攻击位置
        targetPos = new Vector3(currentEnemy.attacker.position.x, currentEnemy.attacker.position.y, 0);

        // 判断攻击距离
        if (Mathf.Abs(targetPos.x - currentEnemy.transform.position.x) < attack.attackRange
            && Mathf.Abs(targetPos.y - currentEnemy.transform.position.y) < attack.attackRange)
        {
            // 攻击
            isAttack = true;
            if (!currentEnemy.isHurt && isAttack)
            {
                currentEnemy.rb.velocity = Vector2.zero;
                currentEnemy.animator.SetBool("run", false);
                currentEnemy.animator.SetBool("walk", false);
            }

            // 计时器判断
            if (attackRateCounter <= 0)
            {
                currentEnemy.animator.SetTrigger("attack");
                attackRateCounter = attack.attackRate;      // 重置
            }
        }
        // 超出攻击范围
        else
        {
            isAttack = false;
            currentEnemy.animator.SetBool("walk", true);
        }
    }

    public override void PhysicsUpdate()
    {
        if (!currentEnemy.wait && !currentEnemy.isHurt && !currentEnemy.isDead&&!isAttack)
            currentEnemy.rb.velocity = new Vector2(-currentEnemy.faceDir.x * currentEnemy.currentSpeed * Time.deltaTime, currentEnemy.rb.velocity.y);
        else
            currentEnemy.rb.velocity = Vector2.zero;
    }
    public override void OnExit()
    {
        currentEnemy.animator.SetBool("run", false);
        Debug.Log("Exit Chase");
    }
}
