using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : MonoBehaviour
{
    [Header("基本属性")]
    public int damage;      // 伤害值
    public float attackRange;     // 攻击范围
    public int attackRate;      // 攻击频率

    private void OnTriggerStay2D(Collider2D other)
    {
        /*?--相当于!null，也就是不为空判断*/
        other.GetComponent<Charactor>()?.TakeDamage(this);
    }
}
