using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boar : Enemy
{
    /// <summary>
    /// 因为Enemy当中的PratrolState和ChaseState都是通用的，没有具体到哪个敌人
    /// 所以需要在Boar.py当中重写Enemy的Awake方法，具体赋予状态值
    /// </summary>
    protected override void Awake()
    {
        base.Awake();
        // 野猪巡逻状态
        patrolState = new BoarPratrolState();       
        chaseState = new BoarChaseState();
    }
}
