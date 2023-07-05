using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public PlayerInputControl inputAction;      //������ϵͳ
    private Rigidbody2D rb;
    private PhysicsCheck physicsCheck;
    private CapsuleCollider2D coll;         // ��ײ��

    private int faceDir;        //��ǰ��������

    [Header("��������")]
    public Vector2 inputDirection;      //���뷽��
    // �ٶ�
    public int walkSpeed;
    public int runSpeed;
    // ��ײ���ʼ����
    private Vector2 originOffset;
    private Vector2 originSize;

    public float jumpForce;     // ��Ծ��

    [Header("״̬")]
    public bool isRun;
    public bool isCrouch;

    private void Awake()
    {
        inputAction = new PlayerInputControl();

        rb = GetComponent<Rigidbody2D>();
        physicsCheck = GetComponent<PhysicsCheck>();

        //��ײ���ʼ��
        coll = GetComponent<CapsuleCollider2D>();
        originOffset = coll.offset;
        originSize = coll.size;

        // started--Action<InputAction.CallbackContext>����ʵ����һ��Actionί�У���ί�����Jump����
        inputAction.Gameplay.Jump.started += Jump;

        #region ǿ���ܲ�
        /*
         ctx--�ص���������Լ����ռ�
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
        //�ƶ�
        Move();
    }

    /// <summary>
    /// �����ƶ�
    /// </summary>
    public void Move()
    {
        // �¶�ʱ��ֹ�ƶ�
        if (!isCrouch)
        {
            if (isRun == false)
                // ���ﲽ���ٶ�����
                rb.velocity = new Vector2(inputDirection.x * walkSpeed * Time.fixedDeltaTime, rb.velocity.y);
            else
                // �����ܲ��ٶ�����
                rb.velocity = new Vector2(inputDirection.x * runSpeed * Time.fixedDeltaTime, rb.velocity.y);
        }

        // ��ʼ�����ﵱǰ������ֵΪ1
        faceDir = (int)transform.localScale.x;

        if (inputDirection.x > 0)
            faceDir = 1;
        else if(inputDirection.x < 0)     
            faceDir = -1;

        // ���﷭ת
        transform.localScale = new Vector3(faceDir, 1, 1);

        #region �����¶�
        isCrouch = inputDirection.y < -0.5f && physicsCheck.isGround;
        if (isCrouch) 
        {
            // �޸���ײ��λ�úʹ�С
            coll.offset = new Vector2(-0.07f, 0.85f);
            coll.size = new Vector2(0.6f, 1.7f);
        }
        else
        {
            // ��ԭ��ʼ��ײ�����
            coll.offset = originOffset;
            coll.size = originSize;
        }
        #endregion
    }

    /// <summary>
    /// ������Ծ
    /// </summary>
    /// <param name="obj"></param>
    /// <exception cref="NotImplementedException"></exception>
    private void Jump(InputAction.CallbackContext obj)
    {
        if(physicsCheck.isGround)
          rb.AddForce(transform.up * jumpForce, ForceMode2D.Impulse);
    }
}
