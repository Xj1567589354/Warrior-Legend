using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [Header("�����¼�")]
    public SceneLoaderEventSO sceneLoadEvent;       // ��������ִ�е��¼�
    public VoidEventSO afterSceneLoadedEvent;
    public VoidEventSO loadDataEvent;               // ������Ϸ������ִ�е��¼�
    public VoidEventSO backToMenuEvent;             // �������˵���ִ�е��¼�

    public PlayerInputControl inputAction;      //������ϵͳ
    private Rigidbody2D rb;
    private PhysicsCheck physicsCheck;
    private CapsuleCollider2D coll;         // ��ײ��
    private PlayerAnimation playerAnimation;
    private Charactor charactor;

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
    public float wallJumpForce; // ��ǽ����
    public float slideDistance; // ��������
    public float slideSpeed;    // �����ٶ�
    public int slidePowerCost;  // ��������ֵ

    [Header("״̬")]
    public bool isRun;
    public bool isCrouch;
    public bool isHurt;
    public bool isDead;
    public bool isAttack;
    public bool walkJump;
    public bool isSlide;

    [Header("�������")]
    public PhysicsMaterial2D normal;    // ��Ħ����
    public PhysicsMaterial2D wall;      // ��Ħ����

    private void Awake()
    {
        inputAction = new PlayerInputControl();
        playerAnimation = GetComponent<PlayerAnimation>();

        rb = GetComponent<Rigidbody2D>();
        physicsCheck = GetComponent<PhysicsCheck>();
        charactor = GetComponent<Charactor>();

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
        inputAction.Gameplay.Run.performed += ctx => { 
            isRun = true;
        };
        inputAction.Gameplay.Run.canceled += ctx =>
        {
            isRun = false;
        };
        #endregion

        // ����
        inputAction.Gameplay.Attack.started += PlayerAttack;

        // ����
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

        CheckState();   // ״̬���
    }

    private void FixedUpdate()
    {   
        if(!isHurt)
            //�ƶ�
            Move();
    }

    /// <summary>
    /// ��ȡ��Ϸ����
    /// </summary>
    private void OnLoadDataEvent()
    {
        isDead = false;
    }

    /// <summary>
    /// �տ�ʼ���س���ִ�е��¼�
    /// </summary>
    /// <param name="arg0"></param>
    /// <param name="arg1"></param>
    /// <param name="arg2"></param>
    private void OnLoadEvent(GameSceneSO arg0, Vector3 arg1, bool arg2)
    {
        inputAction.Gameplay.Disable();     // ��������ϵͳ
    }

    /// <summary>
    /// ���س���֮��ִ�е��¼�
    /// </summary>
    private void OnAfterSceneLoadedEvent()
    {
        inputAction.Gameplay.Enable();     // ��������ϵͳ
    }


    /// <summary>
    /// �����ƶ�
    /// </summary>
    public void Move()
    {
        if (!isAttack)
        {
            // �¶�ʱ��ֹ�ƶ�
            if (!isCrouch && !walkJump)
            {
                if (isRun == false)
                    // ���ﲽ���ٶ�����
                    rb.velocity = new Vector2(inputDirection.x * walkSpeed * Time.deltaTime, rb.velocity.y);
                else
                    // �����ܲ��ٶ�����
                    rb.velocity = new Vector2(inputDirection.x * runSpeed * Time.deltaTime, rb.velocity.y);
            }

            // ��ʼ�����ﵱǰ������ֵΪ1
            faceDir = (int)transform.localScale.x;

            if (inputDirection.x > 0)
                faceDir = 1;
            else if (inputDirection.x < 0)
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
        else
        {
            rb.velocity = new Vector2(transform.localScale.x * 0.8f, rb.velocity.y);
        }
    }

    /// <summary>
    /// ������Ծ
    /// </summary>
    /// <param name="obj"></param>
    /// <exception cref="NotImplementedException"></exception>
    private void Jump(InputAction.CallbackContext obj)
    {
        if (physicsCheck.isGround)
        {
            rb.AddForce(transform.up * jumpForce, ForceMode2D.Impulse);
            //GetComponent<AudioDefination>().PlayAudioClip();        // ������Ч

            // ��ֹ����Э��
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
    /// ��ҹ���
    /// </summary>
    /// <param name="obj"></param>
    private void PlayerAttack(InputAction.CallbackContext obj)
    {
        playerAnimation.PlayAttack();
        isAttack = true;
    }

    /// <summary>
    /// ����
    /// </summary>
    /// <param name="obj"></param>
    /// <exception cref="NotImplementedException"></exception>
    private void Slide(InputAction.CallbackContext obj)
    {
        if (!isSlide && physicsCheck.isGround && (charactor.currentPower >= slidePowerCost))
        {
            isSlide = true;

            // ����Ŀ���
            var targetPos = new Vector3(transform.position.x + slideDistance * transform.localScale.x, transform.position.y);

            // �����޵�֡
            gameObject.layer = LayerMask.NameToLayer("Enemy");
            // ��ʼЭ��
            StartCoroutine(TriggerSilde(targetPos));

            // ����ֵ����
            charactor.OnSlide(slidePowerCost);
        }
    }

    /// <summary>
    /// ����Э��
    /// </summary>
    /// <param name="_targetPose">Ŀ��λ��</param>
    /// <returns></returns>
    private IEnumerator TriggerSilde(Vector3 _targetPose)
    {
        do
        {
            yield return null;
            // �ж��Ƿ񿿽�����
            if (!physicsCheck.isGround)
                break;
            // �ж��Ƿ񿿽�ǽ��
            if((physicsCheck.isTouchLeftWall && transform.localScale.x < 0.0f) || (physicsCheck.isTouchRightWall && transform.localScale.x > 0.0f))
            {
                isSlide = false;
                break;
            }

            // ����ǰ��
            rb.MovePosition(new Vector2(transform.position.x + transform.localScale.x * slideSpeed, transform.position.y));
        }
        while (MathF.Abs(_targetPose.x - transform.position.x) > 0.1f);

        isSlide = false;
        gameObject.layer = LayerMask.NameToLayer("Player");
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
        if (isDead || isSlide)
             /*
             LayerMask.NameToLayer("Enemy")--��ȡEnemy���index,�����gameObject.layer
             */
             gameObject.layer = LayerMask.NameToLayer("Enemy");
        else
             gameObject.layer = LayerMask.NameToLayer("Player");

        // �����Ƿ��ڵ���ѡ��ͬ�������
        coll.sharedMaterial = physicsCheck.isGround ? normal : wall;

        // ���Ƶ�ǽ���ٶ�
        if (physicsCheck.onWall)
            rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y / 2.0f);
        else
            rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y);

        if(walkJump && rb.velocity.y < 0.0f)
            walkJump = false;
    }
}
