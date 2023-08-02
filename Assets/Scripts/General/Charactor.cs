using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Charactor : MonoBehaviour
{
    public bool isPlayer;

    [Header("��������")]
    public float maxHealth;       // ���Ѫ��
    public float currentHealth;   // ��ǰѪ��
    public float maxPower;        // �����ֵ
    public float currentPower;    // ��ǰ����ֵ
    public float powerRecoverSpeed;   //����ֵ�ָ��ٶ�
    private PlayerController controller;

    [Header("�����޵�")]
    public float invulnerableDuration;      // �޵�ʱ��
    [HideInInspector]public float invulnerableCounter;        // �޵м�����
    public bool invulnerable;       // �޵�״̬

    [Header("Unity�¼�")]
    public UnityEvent<Charactor> onHealthChange;  // Ѫ���¼�
    public UnityEvent<Transform> onTakeDamage;    // �����¼�
    public UnityEvent onDie;        // �����¼�

    private void Start()
    {
        currentHealth = maxHealth;
        currentPower = maxPower;
        // ��ʼ��Ѫ��
        onHealthChange?.Invoke(this);

        controller = GetComponent<PlayerController>();
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

        // �������ָ�
        if (isPlayer && !controller.isSlide && (currentPower < maxPower))
            currentPower += Time.deltaTime * powerRecoverSpeed;
    }

    public void TakeDamage(Attack attacker)
    {
        if (invulnerable)
            return;

        // ȷ����ǰѪ������С��0
        if(currentHealth - attacker.damage > 0)
        {
            currentHealth -= attacker.damage;

            //ִ������
            onTakeDamage?.Invoke(attacker.transform);
            TriggerInvulneral();    // �����޵�
        }
        else
        {
            currentHealth = 0;
            // ��������
            onDie?.Invoke();
        }

        onHealthChange?.Invoke(this);
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


    public void OnSlide(int _cost)
    {
        currentPower -= _cost;
        onHealthChange?.Invoke(this);
    }
}   
