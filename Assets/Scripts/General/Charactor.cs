using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Charactor : MonoBehaviour
{
    [Header("基本属性")]
    public int maxHealth;       // 最大血量
    public int currentHealth;   // 当前血量

    [Header("攻击无敌")]
    public float invulnerableDuration;      // 无敌时间
    private float invulnerableCounter;        // 无敌计数器
    public bool invulnerable;       // 无敌状态

    [Header("Unity事件")]
    public UnityEvent<Transform> onTakeDamage;    // 受伤事件
    public UnityEvent onDie;        // 死亡事件

    private void Start()
    {
        currentHealth = maxHealth;
    }

    private void Update()
    {
        if (invulnerable)
        {
            // 无敌计时器计时
            invulnerableCounter -= Time.deltaTime;

            if(invulnerableCounter <= 0)
                invulnerable = false;
        }
    }

    public void TakeDamage(Attack attacker)
    {
        if (invulnerable)
            return;

        // 确保当前血量不会小于0
        if(currentHealth - attacker.damage > 0)
        {
            currentHealth -= attacker.damage;
            TriggerInvulneral();    // 开启无敌

            //执行受伤
            onTakeDamage?.Invoke(attacker.transform);
        }
        else
        {
            currentHealth = 0;
            // 触发死亡
            onDie?.Invoke();
        }

    }

    /// <summary>
    /// 开启无敌
    /// </summary>
    public void TriggerInvulneral()
    {
        if (!invulnerable)
        {
            invulnerable = true;
            invulnerableCounter = invulnerableDuration;
        }
    }
}   
