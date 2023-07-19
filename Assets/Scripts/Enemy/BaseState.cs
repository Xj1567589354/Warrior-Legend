public abstract class BaseState
{
    protected Enemy currentEnemy;       //  当前敌人
    public abstract void OnEnter(Enemy enemy);      // 进入方法
    public abstract void OnExit();      // 退出方法
    public abstract void LogicUpdate();     // 逻辑更新方法
    public abstract void PhysicsUpdate();       // 物理更新方法
}
