using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boar : Enemy
{
    public override void Move()
    {
        base.Move();
        animator.SetBool("walk", true);
    }
}
