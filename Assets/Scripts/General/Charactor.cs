using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Charactor : MonoBehaviour, ISaveable
{
    [Header("�¼�����")]
    public VoidEventSO newGameEvent;

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
    // UnityEvent�ܹ�֪ͨ�ܶ�����ע��ĺ�������
    public UnityEvent<Charactor> onHealthChange;  // Ѫ���¼�
    public UnityEvent<Transform> onTakeDamage;    // �����¼�
    public UnityEvent onDie;        // �����¼�


    private void Start()
    {
        currentHealth = maxHealth;
        controller = GetComponent<PlayerController>();
    }
    private void NewGame()
    {
        currentHealth = maxHealth;
        currentPower = maxPower;
        // ��ʼ��Ѫ��
        onHealthChange?.Invoke(this);
    }

    private void OnEnable()
    {
        newGameEvent.onEventRaised += NewGame;
        // ����RegisterSaveData����
        ISaveable saveable = this;
        saveable.RegisterSaveData();
    }

    private void OnDisable()
    {
        newGameEvent.onEventRaised -= NewGame;
        // ����UnRegisterSaveData����
        ISaveable saveable = this;
        saveable.UnRegisterSaveData();
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
        {
            currentPower += Time.deltaTime * powerRecoverSpeed;
        }
            
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Water")){
            currentHealth = 0;              // ��ǰѪ����Ϊ0
            onHealthChange?.Invoke(this);   // ����Ѫ��
            onDie?.Invoke();                // ����
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

    /// <summary>
    /// ����GUID
    /// </summary>
    /// <returns></returns>
    /// <exception cref="System.NotImplementedException"></exception>
    public DataDefinition GetDataID()
    {
        return GetComponent<DataDefinition>();
    }

    /// <summary>
    /// �������
    /// </summary>
    /// <exception cref="System.NotImplementedException"></exception>
    public void GetSaveData(Data data)
    {
        // �ж�Data�����ֵ䵱���Ƿ������ǰ����GUID
        if (data.characterPosDict.ContainsKey(GetDataID().ID))
            data.characterPosDict[GetDataID().ID] = transform.position;
        else
            data.characterPosDict.Add(GetDataID().ID, transform.position);
    }

    /// <summary>
    /// ��������
    /// </summary>
    /// <param name="data"></param>
    /// <exception cref="System.NotImplementedException"></exception>
    public void LoadData(Data data)
    {
        if (data.characterPosDict.ContainsKey(GetDataID().ID))
            transform.position = data.characterPosDict[GetDataID().ID];
    }
}   
