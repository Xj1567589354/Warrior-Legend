using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Charactor : MonoBehaviour
{
    [Header("��������")]
    public int maxHealth;       // ���Ѫ��
    public int currentHealth;   // ��ǰѪ��

    [Header("�����޵�")]
    public float invulnerableDuration;      // �޵�ʱ��
    private float invulnerableCounter;        // �޵м�����
    public bool invulnerable;       // �޵�״̬

    [Header("Unity�¼�")]
    public UnityEvent<Transform> onTakeDamage;    // �����¼�
    public UnityEvent onDie;        // �����¼�

    private void Start()
    {
        currentHealth = maxHealth;
    }

    private void Update()
    {
        if (invulnerable)
        {
            // �޵м�ʱ����ʱ
            invulnerableCounter -= Time.deltaTime;

            if(invulnerableCounter <= 0)
                invulnerable = false;
        }
    }

    public void TakeDamage(Attack attacker)
    {
        if (invulnerable)
            return;

        // ȷ����ǰѪ������С��0
        if(currentHealth - attacker.damage > 0)
        {
            currentHealth -= attacker.damage;
            TriggerInvulneral();    // �����޵�

            //ִ������
            onTakeDamage?.Invoke(attacker.transform);
        }
        else
        {
            currentHealth = 0;
            // ��������
            onDie?.Invoke();
        }

    }

    /// <summary>
    /// �����޵�
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
