using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public PlayerInputControl inputAction;      //新输入系统
    private Rigidbody2D rb;
    private PhysicsCheck physicsCheck;
    private CapsuleCollider2D coll;         // 碰撞体

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

    [Header("状态")]
    public bool isRun;
    public bool isCrouch;

    private void Awake()
    {
        inputAction = new PlayerInputControl();

        rb = GetComponent<Rigidbody2D>();
        physicsCheck = GetComponent<PhysicsCheck>();

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
        inputAction.Gameplay.Run.started += ctx => { 
            isRun = true;
        };
        inputAction.Gameplay.Run.canceled += ctx =>
        {
            isRun = false;
        };
        #endregion
    }

    private void OnEnable()
    {
        inputAction.Enable();
    }

    private void OnDisable()
    {
        inputAction.Disable();
    }

    private void Update()
    {
        inputDirection = inputAction.Gameplay.Move.ReadValue<Vector2>();
    }

    private void FixedUpdate()
    {   
        //移动
        Move();
    }

    /// <summary>
    /// 人物移动
    /// </summary>
    public void Move()
    {
        // 下蹲时禁止移动
        if (!isCrouch)
        {
            if (isRun == false)
                // 人物步行速度设置
                rb.velocity = new Vector2(inputDirection.x * walkSpeed * Time.fixedDeltaTime, rb.velocity.y);
            else
                // 人物跑步速度设置
                rb.velocity = new Vector2(inputDirection.x * runSpeed * Time.fixedDeltaTime, rb.velocity.y);
        }

        // 初始化人物当前面向，数值为1
        faceDir = (int)transform.localScale.x;

        if (inputDirection.x > 0)
            faceDir = 1;
        else if(inputDirection.x < 0)     
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

    /// <summary>
    /// 人物跳跃
    /// </summary>
    /// <param name="obj"></param>
    /// <exception cref="NotImplementedException"></exception>
    private void Jump(InputAction.CallbackContext obj)
    {
        if(physicsCheck.isGround)
          rb.AddForce(transform.up * jumpForce, ForceMode2D.Impulse);
    }
}
