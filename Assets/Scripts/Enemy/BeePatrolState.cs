using UnityEngine;

public class BeePatrolState : BaseState
{
    private Vector3 targetPos;      // �ƶ�Ŀ��λ��
    private Vector3 moveDir;        // �ƶ�����
    public override void OnEnter(Enemy enemy)
    {
        currentEnemy = enemy;
        currentEnemy.currentSpeed = currentEnemy.normalSpeed;
        targetPos = currentEnemy.GetNewPoint();     // �õ��µ��ƶ�Ŀ���
    }
    public override void LogicUpdate()
    {
        // �Ƿ��ֵ���
        if (currentEnemy.FoundPlayer())
            currentEnemy.SwitchState(NPCState.Chase);

        // �Ƿ񵽴��ƶ�Ŀ���
        if(Mathf.Abs(targetPos.x - currentEnemy.transform.position.x) < 0.1f
            && Mathf.Abs(targetPos.y - currentEnemy.transform.position.y) < 0.1f)
        {
            currentEnemy.wait = true;
            targetPos = currentEnemy.GetNewPoint();     // �õ��µ��ƶ�Ŀ���
        }

        // ����ƶ�����
        moveDir = (targetPos - currentEnemy.transform.position).normalized;

        // �ı��泯����
        if (moveDir.x > 0)
            currentEnemy.transform.localScale = new Vector3(-1, 1, 1);
        if (moveDir.x < 0)
            currentEnemy.transform.localScale = new Vector3(1, 1, 1);
    }

    public override void PhysicsUpdate()
    {
        // �ƶ�
        if (!currentEnemy.wait && !currentEnemy.isHurt && !currentEnemy.isDead)
            currentEnemy.rb.velocity = moveDir * currentEnemy.currentSpeed * Time.deltaTime;
        else
            currentEnemy.rb.velocity = Vector2.zero;
    }
    public override void OnExit()
    {

    }
}
