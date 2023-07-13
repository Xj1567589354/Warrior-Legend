using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Metadata.W3cXsd2001;
using UnityEngine;
using UnityEngine.UIElements;

public class Enemy : MonoBehaviour
{
    Rigidbody2D rb;
    protected Animator animator;
    private PhysicsCheck physicsCheck;

    [Header("基本参数")]
    public float normalSpeed;   // 移动速度
    public float chaseSpeed;    // 追击速度
    public float currentSpeed;  // 当前速度
    public float hurtForce;     // 受伤力
    public Vector3 faceDir;     // 面朝方向
    public Vector2 hurtDir;     // 受伤方向

    private Transform attacker;  // 攻击者

    [Header("计时器")]
    public float waitTime;      // 等待时间
    public float waitTimeCounter;   // 计时器
    public bool wait;       // 等待状态

    [Header("状态")]
    public bool isHurt;     // 受伤
    public bool isDead;     // 死亡

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();

        currentSpeed = normalSpeed;
        physicsCheck = GetComponent<PhysicsCheck>();

        // 初始化计时器
        waitTimeCounter = waitTime;
    }

    private void Update()
    {
        // 获取方向
        faceDir = new Vector3(-transform.localScale.x, 0, 0);

        // 检测是否碰到墙体，随之翻转
        /*判断条件是为了防止角色转身之后尾部再次检测到墙体，从而导致两次检测转身*/
        if ((physicsCheck.isTouchLeftWall&&faceDir.x<0) 
            || (physicsCheck.isTouchRightWall&&faceDir.x>0))
        {
            wait = true;
        }

        TimeCounter();      // 计时器
    }

    private void FixedUpdate()
    {
        // 移动
        Move();
    }

    public virtual void Move()
    {
        if(!isHurt && !isDead)
            rb.velocity = new Vector2(faceDir.x * currentSpeed * Time.fixedDeltaTime, rb.velocity.y);
    }

    /// <summary>
    /// 计时器
    /// </summary>
    public void TimeCounter()
    {
        if (wait)
        {
            animator.SetBool("walk", false);        // 进入idle状态
            waitTimeCounter -= Time.deltaTime;
            if(waitTimeCounter <= 0)
            {
                wait = false;
                waitTimeCounter = waitTime;     // 重置计时器
                transform.localScale = new Vector3(faceDir.x, 1, 1);       // 转身
            }
        }
    }

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
}
