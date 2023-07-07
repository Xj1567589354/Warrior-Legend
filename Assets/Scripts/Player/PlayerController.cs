using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public PlayerInputControl inputAction;      //������ϵͳ
    private Rigidbody2D rb;
    private PhysicsCheck physicsCheck;
    private CapsuleCollider2D coll;         // ��ײ��
    private PlayerAnimation playerAnimation;

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
    public float hurtForce;     // ������

    [Header("״̬")]
    public bool isRun;
    public bool isCrouch;
    public bool isHurt;
    public bool isDead;
    public bool isAttack;

    [Header("�������")]
    public PhysicsMaterial2D normal;    // ��Ħ����
    public PhysicsMaterial2D wall;      // ��Ħ����

    private void Awake()
    {
        inputAction = new PlayerInputControl();
        playerAnimation = GetComponent<PlayerAnimation>();

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

        // ����
        inputAction.Gameplay.Attack.started += PlayerAttack;
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

        CheckState();   // ״̬���
    }

    private void FixedUpdate()
    {   
        if(!isHurt&&!isAttack)
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

    /// <summary>
    /// ��ҹ���
    /// </summary>
    /// <param name="obj"></param>
    private void PlayerAttack(InputAction.CallbackContext obj)
    {
        playerAnimation.PlayAttack();
        isAttack = true;
    }


    /// <summary>
    /// ���㷴��
    /// </summary>
    /// <param name="attacker">������λ��</param>
    public void GetHurt(Transform attacker)
    {
        isHurt = true;
        rb.velocity = Vector2.zero;     // �ٶȹ���
        Vector2 dir = new Vector2((transform.position.x - attacker.position.x), 0).normalized;      // ���㷽�򣬹�һ��
        rb.AddForce(dir*hurtForce, ForceMode2D.Impulse);    // ��ӷ�����
    }

    /// <summary>
    /// ִ������
    /// </summary>
    public void PlayDead()
    {
        isDead = true;
        inputAction.Gameplay.Disable();     // ��ֹ����
    }

    /// <summary>
    /// ״̬���
    /// </summary>
    private void CheckState()
    {
        if (isDead)
            /*
             LayerMask.NameToLayer("Enemy")--��ȡEnemy���index,�����gameObject.layer
             */
            gameObject.layer = LayerMask.NameToLayer("Enemy");
        else
            gameObject.layer = LayerMask.NameToLayer("Player");

        // �����Ƿ��ڵ���ѡ��ͬ�������
        coll.sharedMaterial = physicsCheck.isGround ? normal : wall;
    }
}
