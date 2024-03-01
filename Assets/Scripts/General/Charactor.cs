using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Charactor : MonoBehaviour, ISaveable
{
    [Header("事件监听")]
    public VoidEventSO newGameEvent;

    public bool isPlayer;

    [Header("基本属性")]
    public float maxHealth;       // 最大血量
    public float currentHealth;   // 当前血量
    public float maxPower;        // 最大精力值
    public float currentPower;    // 当前精力值
    public float powerRecoverSpeed;   //精力值恢复速度
    private PlayerController controller;

    [Header("攻击无敌")]
    public float invulnerableDuration;      // 无敌时间
    [HideInInspector]public float invulnerableCounter;        // 无敌计数器
    public bool invulnerable;       // 无敌状态

    [Header("Unity事件")]
    // UnityEvent能够通知很多在其注册的函数方法
    public UnityEvent<Charactor> onHealthChange;  // 血条事件
    public UnityEvent<Transform> onTakeDamage;    // 受伤事件
    public UnityEvent onDie;        // 死亡事件


    private void Start()
    {
        currentHealth = maxHealth;
        controller = GetComponent<PlayerController>();
    }
    private void NewGame()
    {
        currentHealth = maxHealth;
        currentPower = maxPower;
        // 初始化血条
        onHealthChange?.Invoke(this);
    }

    private void OnEnable()
    {
        newGameEvent.onEventRaised += NewGame;
        // 调用RegisterSaveData方法
        ISaveable saveable = this;
        saveable.RegisterSaveData();
    }

    private void OnDisable()
    {
        newGameEvent.onEventRaised -= NewGame;
        // 调用UnRegisterSaveData方法
        ISaveable saveable = this;
        saveable.UnRegisterSaveData();
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

        // 能量条恢复
        if (isPlayer && !controller.isSlide && (currentPower < maxPower))
        {
            currentPower += Time.deltaTime * powerRecoverSpeed;
        }
            
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Water")){
            currentHealth = 0;              // 当前血量设为0
            onHealthChange?.Invoke(this);   // 更新血量
            onDie?.Invoke();                // 死亡
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

            //执行受伤
            onTakeDamage?.Invoke(attacker.transform);
            TriggerInvulneral();    // 开启无敌
        }
        else
        {
            currentHealth = 0;
            // 触发死亡
            onDie?.Invoke();
        }

        onHealthChange?.Invoke(this);
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


    public void OnSlide(int _cost)
    {
        currentPower -= _cost;
        onHealthChange?.Invoke(this);
    }

    /// <summary>
    /// 返回GUID
    /// </summary>
    /// <returns></returns>
    /// <exception cref="System.NotImplementedException"></exception>
    public DataDefinition GetDataID()
    {
        return GetComponent<DataDefinition>();
    }

    /// <summary>
    /// 获得数据
    /// </summary>
    /// <exception cref="System.NotImplementedException"></exception>
    public void GetSaveData(Data data)
    {
        // 判断Data类中字典当中是否包含当前对象GUID
        if (data.characterPosDict.ContainsKey(GetDataID().ID))
            data.characterPosDict[GetDataID().ID] = transform.position;
        else
            data.characterPosDict.Add(GetDataID().ID, transform.position);
    }

    /// <summary>
    /// 加载数据
    /// </summary>
    /// <param name="data"></param>
    /// <exception cref="System.NotImplementedException"></exception>
    public void LoadData(Data data)
    {
        if (data.characterPosDict.ContainsKey(GetDataID().ID))
            transform.position = data.characterPosDict[GetDataID().ID];
    }
}   
