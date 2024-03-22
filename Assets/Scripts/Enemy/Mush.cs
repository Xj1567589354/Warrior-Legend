using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mush : Enemy
{
    /// <summary>
    /// 因为Enemy当中的PratrolState和ChaseState都是通用的，没有具体到哪个敌人
    /// 所以需要在Boar.py当中重写Enemy的Awake方法，具体赋予状态值
    /// </summary>
    protected override void Awake()
    {
        base.Awake();
        // 蘑菇人追击状态
        patrolState = new MushChaseState();
    }

    public override void Move()
    {}
}
