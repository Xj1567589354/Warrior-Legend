using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [Header("监听事件")]
    public SceneLoaderEventSO sceneLoadEvent;       // 场景加载执行的事件
    public VoidEventSO afterSceneLoadedEvent;
    public VoidEventSO loadDataEvent;               // 加载游戏进度所执行的事件
    public VoidEventSO backToMenuEvent;             // 返回主菜单所执行的事件

    public PlayerInputControl inputAction;      //新输入系统
    private Rigidbody2D rb;
    private PhysicsCheck physicsCheck;
    private CapsuleCollider2D coll;         // 碰撞体
    private PlayerAnimation playerAnimation;
    private Charactor charactor;

    private int faceDir;        //当前人物面向

    [Header("基本参数")]
    public Vector2 inputDirection;      //输入方向
    // 速度
    public int walkSpeed;
    public int runSpeed;
    // 碰撞体初始参数
    private Vector2 originOffset;
    private Vector2 originSize;

    public float jumpForce;     // 跳跃力
    public float hurtForce;     // 反弹力
    public float wallJumpForce; // 蹬墙跳力
    public float slideDistance; // 滑铲距离
    public float slideSpeed;    // 滑铲速度
    public int slidePowerCost;  // 滑铲消耗值

    [Header("状态")]
    public bool isRun;
    public bool isCrouch;
    public bool isHurt;
    public bool isDead;
    public bool isAttack;
    public bool walkJump;
    public bool isSlide;

    [Header("物理材质")]
    public PhysicsMaterial2D normal;    // 有摩擦力
    public PhysicsMaterial2D wall;      // 无摩擦力

    private void Awake()
    {
        inputAction = new PlayerInputControl();
        playerAnimation = GetComponent<PlayerAnimation>();

        rb = GetComponent<Rigidbody2D>();
        physicsCheck = GetComponent<PhysicsCheck>();
        charactor = GetComponent<Charactor>();

        //碰撞体初始化
        coll = GetComponent<CapsuleCollider2D>();
        originOffset = coll.offset;
        originSize = coll.size;

        // started--Action<InputAction.CallbackContext>，其实就是一个Action委托，给委托添加Jump方法
        inputAction.Gameplay.Jump.started += Jump;

        #region 强制跑步
        /*
         ctx--回调函数，节约代码空间
        */
        inputAction.Gameplay.Run.performed += ctx => { 
            isRun = true;
        };
        inputAction.Gameplay.Run.canceled += ctx =>
        {
            isRun = false;
        };
        #endregion

        // 攻击
        inputAction.Gameplay.Attack.started += PlayerAttack;

        // 滑铲
        inputAction.Gameplay.Slide.started += Slide;

        inputAction.Enable();
    }

    private void OnEnable()
    {
        sceneLoadEvent.LoadRequestEvent += OnLoadEvent;
        afterSceneLoadedEvent.onEventRaised += OnAfterSceneLoadedEvent;
        loadDataEvent.onEventRaised += OnLoadDataEvent;
        backToMenuEvent.onEventRaised += OnLoadDataEvent;
    }

    private void OnDisable()
    {
        inputAction.Disable();
        sceneLoadEvent.LoadRequestEvent -= OnLoadEvent;
        afterSceneLoadedEvent.onEventRaised -= OnAfterSceneLoadedEvent;
        loadDataEvent.onEventRaised -= OnLoadDataEvent;
        backToMenuEvent.onEventRaised -= OnLoadDataEvent;
    }

    private void Update()
    {
        inputDirection = inputAction.Gameplay.Move.ReadValue<Vector2>();

        CheckState();   // 状态检测
    }

    private void FixedUpdate()
    {   
        if(!isHurt)
            //移动
            Move();
    }

    /// <summary>
    /// 读取游戏进度
    /// </summary>
    private void OnLoadDataEvent()
    {
        isDead = false;
    }

    /// <summary>
    /// 刚开始加载场景执行的事件
    /// </summary>
    /// <param name="arg0"></param>
    /// <param name="arg1"></param>
    /// <param name="arg2"></param>
    private void OnLoadEvent(GameSceneSO arg0, Vector3 arg1, bool arg2)
    {
        inputAction.Gameplay.Disable();     // 禁用输入系统
    }

    /// <summary>
    /// 加载场景之后执行的事件
    /// </summary>
    private void OnAfterSceneLoadedEvent()
    {
        inputAction.Gameplay.Enable();     // 启用输入系统
    }


    /// <summary>
    /// 人物移动
    /// </summary>
    public void Move()
    {
        if (!isAttack)
        {
            // 下蹲时禁止移动
            if (!isCrouch && !walkJump)
            {
                if (isRun == false)
                    // 人物步行速度设置
                    rb.velocity = new Vector2(inputDirection.x * walkSpeed * Time.deltaTime, rb.velocity.y);
                else
                    // 人物跑步速度设置
                    rb.velocity = new Vector2(inputDirection.x * runSpeed * Time.deltaTime, rb.velocity.y);
            }

            // 初始化人物当前面向，数值为1
            faceDir = (int)transform.localScale.x;

            if (inputDirection.x > 0)
                faceDir = 1;
            else if (inputDirection.x < 0)
                faceDir = -1;

            // 人物翻转
            transform.localScale = new Vector3(faceDir, 1, 1);

            #region 人物下蹲
            isCrouch = inputDirection.y < -0.5f && physicsCheck.isGround;
            if (isCrouch)
            {
                // 修改碰撞体位置和大小
                coll.offset = new Vector2(-0.07f, 0.85f);
                coll.size = new Vector2(0.6f, 1.7f);
            }
            else
            {
                // 还原初始碰撞体参数
                coll.offset = originOffset;
                coll.size = originSize;
            }
            #endregion
        }
        else
        {
            rb.velocity = new Vector2(transform.localScale.x * 0.8f, rb.velocity.y);
        }
    }

    /// <summary>
    /// 人物跳跃
    /// </summary>
    /// <param name="obj"></param>
    /// <exception cref="NotImplementedException"></exception>
    private void Jump(InputAction.CallbackContext obj)
    {
        if (physicsCheck.isGround)
        {
            rb.AddForce(transform.up * jumpForce, ForceMode2D.Impulse);
            //GetComponent<AudioDefination>().PlayAudioClip();        // 播放音效

            // 终止滑铲协程
            isSlide = false;
            StopAllCoroutines();
        }
        else if (physicsCheck.onWall)
        {
            rb.AddForce(new Vector2(-inputDirection.x, 2.5f) * wallJumpForce, ForceMode2D.Impulse);
            walkJump = true;
        }
    }

    /// <summary>
    /// 玩家攻击
    /// </summary>
    /// <param name="obj"></param>
    private void PlayerAttack(InputAction.CallbackContext obj)
    {
        playerAnimation.PlayAttack();
        isAttack = true;
    }

    /// <summary>
    /// 滑铲
    /// </summary>
    /// <param name="obj"></param>
    /// <exception cref="NotImplementedException"></exception>
    private void Slide(InputAction.CallbackContext obj)
    {
        if (!isSlide && physicsCheck.isGround && (charactor.currentPower >= slidePowerCost))
        {
            isSlide = true;

            // 滑铲目标点
            var targetPos = new Vector3(transform.position.x + slideDistance * transform.localScale.x, transform.position.y);

            // 滑铲无敌帧
            gameObject.layer = LayerMask.NameToLayer("Enemy");
            // 开始协程
            StartCoroutine(TriggerSilde(targetPos));

            // 能量值消耗
            charactor.OnSlide(slidePowerCost);
        }
    }

    /// <summary>
    /// 滑铲协程
    /// </summary>
    /// <param name="_targetPose">目标位置</param>
    /// <returns></returns>
    private IEnumerator TriggerSilde(Vector3 _targetPose)
    {
        do
        {
            yield return null;
            // 判断是否靠近悬崖
            if (!physicsCheck.isGround)
                break;
            // 判断是否靠近墙体
            if((physicsCheck.isTouchLeftWall && transform.localScale.x < 0.0f) || (physicsCheck.isTouchRightWall && transform.localScale.x > 0.0f))
            {
                isSlide = false;
                break;
            }

            // 滑铲前进
            rb.MovePosition(new Vector2(transform.position.x + transform.localScale.x * slideSpeed, transform.position.y));
        }
        while (MathF.Abs(_targetPose.x - transform.position.x) > 0.1f);

        isSlide = false;
        gameObject.layer = LayerMask.NameToLayer("Player");
    }

    /// <summary>
    /// 计算反弹
    /// </summary>
    /// <param name="attacker">攻击者位置</param>
    public void GetHurt(Transform attacker)
    {
        isHurt = true;
        rb.velocity = Vector2.zero;     // 速度归零
        Vector2 dir = new Vector2((transform.position.x - attacker.position.x), 0).normalized;      // 计算方向，归一化
        rb.AddForce(dir*hurtForce, ForceMode2D.Impulse);    // 添加反弹力
    }

    /// <summary>
    /// 执行死亡
    /// </summary>
    public void PlayDead()
    {
        isDead = true;
        inputAction.Gameplay.Disable();     // 禁止输入
    }

    /// <summary>
    /// 状态检测
    /// </summary>
    private void CheckState()
    {
        if (isDead || isSlide)
             /*
             LayerMask.NameToLayer("Enemy")--获取Enemy层的index,赋予给gameObject.layer
             */
             gameObject.layer = LayerMask.NameToLayer("Enemy");
        else
             gameObject.layer = LayerMask.NameToLayer("Player");

        // 根据是否在地面选择不同物理材质
        coll.sharedMaterial = physicsCheck.isGround ? normal : wall;

        // 限制蹬墙跳速度
        if (physicsCheck.onWall)
            rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y / 2.0f);
        else
            rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y);

        if(walkJump && rb.velocity.y < 0.0f)
            walkJump = false;
    }
}
