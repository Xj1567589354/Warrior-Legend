using System.Collections;
using System.Collections.Generic;
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
    public Vector3 faceDir;     // 面朝方向

    [Header("计时器")]
    public float waitTime;      // 等待时间
    public float waitTimeCounter;   // 计时器
    public bool wait;       // 等待状态

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
}
