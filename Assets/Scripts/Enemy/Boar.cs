using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boar : Enemy
{
    protected override void Awake()
    {
        base.Awake();
        // Ұ��Ѳ��״̬
        patrolState = new BoarPratrolState();       
        chaseState = new BoarChaseState();
    }
}
