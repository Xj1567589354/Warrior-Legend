public abstract class BaseState
{
    protected Enemy currentEnemy;       //  ��ǰ����
    public abstract void OnEnter(Enemy enemy);      // ���뷽��
    public abstract void OnExit();      // �˳�����
    public abstract void LogicUpdate();     // �߼����·���
    public abstract void PhysicsUpdate();       // ������·���
}
