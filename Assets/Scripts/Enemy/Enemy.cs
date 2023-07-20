using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Metadata.W3cXsd2001;
using UnityEngine;
using UnityEngine.UIElements;

[RequireComponent(typeof(Rigidbody2D),typeof(Animator),typeof(PhysicsCheck))]
public class Enemy : MonoBehaviour
{
    Rigidbody2D rb;
    [HideInInspector]public Animator animator;
    [HideInInspector]public PhysicsCheck physicsCheck;

    [Header("基本参数")]
    public float normalSpeed;   // 移动速度
    public float chaseSpeed;    // 追击速度
    [HideInInspector] public float currentSpeed;  // 当前速度
    public float hurtForce;     // 受伤力
    [HideInInspector] public Vector3 faceDir;     // 面朝方向
    [HideInInspector] public Vector2 hurtDir;     // 受伤方向

    public Transform attacker;  // 攻击者

    [Header("计时器")]
    public float waitTime;      // 等待时间
    public float waitTimeCounter;   // 等待计时器
    public bool wait;       // 等待状态
    public float lostTime;  // 丢失时间
    public float lostTimeCounter;   // 丢失计时器

    [Header("状态")]
    public bool isHurt;     // 受伤
    public bool isDead;     // 死亡

    [Header("抽象状态")]
    private BaseState currentState;
    protected BaseState patrolState;        // 巡逻状态
    protected BaseState chaseState;         // 追击状态
    protected BaseState skillState;         // 躲藏状态

    [Header("检测")]
    public Vector2 centerOffset;    // 检测偏移
    public Vector2 checkSize;       // 检测大小
    public float checkDistance;     // 检测距离
    public LayerMask attackLayer;   // 图层
            
    protected virtual void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();

        currentSpeed = normalSpeed;
        physicsCheck = GetComponent<PhysicsCheck>();

        // 初始化计时器
        waitTimeCounter = waitTime;
    }

    private void OnEnable()
    {
        currentState = patrolState;
        currentState.OnEnter(this);
    }

    private void Update()
    {
        // 获取方向
        faceDir = new Vector3(-transform.localScale.x, 0, 0);

        currentState.LogicUpdate();
        TimeCounter();      // 计时器
    }

    private void FixedUpdate()
    {
        currentState.PhysicsUpdate();
        if (!isHurt && !isDead && !wait)
            // 移动
            Move();  
    }

    private void OnDisable()
    {
        currentState.OnExit();
    }

    public void Move()
    {
        if(!animator.GetCurrentAnimatorStateInfo(0).IsName("PreMove")&& !animator.GetCurrentAnimatorStateInfo(0).IsName("SnailRecover"))
           rb.velocity = new Vector2(faceDir.x * currentSpeed * Time.deltaTime, rb.velocity.y);
    }

    /// <summary>
    /// 计时器
    /// </summary>
    public void TimeCounter()
    {
        if (wait)
        {
            waitTimeCounter -= Time.deltaTime;
            if(waitTimeCounter <= 0)
            {
                wait = false;
                waitTimeCounter = waitTime;     // 重置计时器
                transform.localScale = new Vector3(faceDir.x, 1, 1);       // 转身
            }
        }

        // 丢失计时部分
        if (!FoundPlayer() && lostTimeCounter > 0)
            lostTimeCounter -= Time.deltaTime;
        else if (FoundPlayer())
            lostTimeCounter = lostTime;
    }

    /// <summary>
    /// 发现敌人
    /// </summary>
    /// <returns>bool</returns>
    public bool FoundPlayer()
    {
       return Physics2D.BoxCast(transform.position + (Vector3)centerOffset, checkSize, 0, faceDir, checkDistance, attackLayer);
    }

    public void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(transform.position + (Vector3)centerOffset + new Vector3((checkDistance * -transform.localScale.x), 0), 0.2f);
    }

    /// <summary>
    /// 转换状态
    /// </summary>
    /// <param name="state">目标状态</param>
    public void SwitchState(NPCState state)
    {
        var newState = state switch
        {
            NPCState.Patrol => patrolState,
            NPCState.Chase => chaseState,
            NPCState.Skill => skillState,
            _=>null
        };
        currentState.OnExit();          // 退出上一状态
        currentState = newState;        // 赋予新状态
        currentState.OnEnter(this);     // 进入新状态
    }

    #region 事件执行方法
    public void OnTakeDamage(Transform attackerTrans)
    {
        // 玩家
        attacker = attackerTrans;    
        // 转身
        if (attacker.position.x - transform.position.x > 0)
            transform.localScale = new Vector3(-1, 1, 1);
        if (attacker.position.x - transform.position.x < 0)
            transform.localScale = new Vector3(1, 1, 1);

        // 受伤击退
        isHurt = true;
        animator.SetTrigger("hurt");
        hurtDir = new Vector2(transform.position.x - attacker.position.x, 0).normalized;
        /*野猪受伤时将x轴上的力停下来，防止野猪原地受伤动画*/
        rb.velocity = new Vector2(0, rb.velocity.y);
        StartCoroutine(OnHurt(hurtDir));
    }
    
    /// <summary>
    /// 协程方法
    /// </summary>
    /// <param name="_hurtDir">受伤方向</param>
    /// <returns></returns>
    private IEnumerator OnHurt(Vector2 _hurtDir)
    {
        rb.AddForce(_hurtDir * hurtForce, ForceMode2D.Impulse);
        // 等待0.45s之后结束受伤状态
        yield return new WaitForSeconds(0.5f);    
        isHurt = false;
    }

    /// <summary>
    /// 死亡
    /// </summary>
    public void OnDie()
    {
        isDead = true;
        animator.SetBool("dead", isDead);
        gameObject.layer = 2;
    }

    public void OnDestroyAfterAnimation()
    {
        Destroy(this.gameObject);
    }
    #endregion
}
