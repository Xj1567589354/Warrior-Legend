using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boar : Enemy
{
    /// <summary>
    /// ��ΪEnemy���е�PratrolState��ChaseState����ͨ�õģ�û�о��嵽�ĸ�����
    /// ������Ҫ��Boar.py������дEnemy��Awake���������帳��״ֵ̬
    /// </summary>
    protected override void Awake()
    {
        base.Awake();
        // Ұ��Ѳ��״̬
        patrolState = new BoarPratrolState();       
        chaseState = new BoarChaseState();
    }
}
