using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Enemy : MonoBehaviour
{
    Rigidbody2D rb;
    protected Animator animator;
    private PhysicsCheck physicsCheck;

    [Header("��������")]
    public float normalSpeed;   // �ƶ��ٶ�
    public float chaseSpeed;    // ׷���ٶ�
    public float currentSpeed;  // ��ǰ�ٶ�
    public Vector3 faceDir;     // �泯����

    [Header("��ʱ��")]
    public float waitTime;      // �ȴ�ʱ��
    public float waitTimeCounter;   // ��ʱ��
    public bool wait;       // �ȴ�״̬

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();

        currentSpeed = normalSpeed;
        physicsCheck = GetComponent<PhysicsCheck>();

        // ��ʼ����ʱ��
        waitTimeCounter = waitTime;
    }

    private void Update()
    {
        // ��ȡ����
        faceDir = new Vector3(-transform.localScale.x, 0, 0);

        // ����Ƿ�����ǽ�壬��֮��ת
        /*�ж�������Ϊ�˷�ֹ��ɫת��֮��β���ٴμ�⵽ǽ�壬�Ӷ��������μ��ת��*/
        if ((physicsCheck.isTouchLeftWall&&faceDir.x<0) 
            || (physicsCheck.isTouchRightWall&&faceDir.x>0))
        {
            wait = true;
        }

        TimeCounter();      // ��ʱ��
    }

    private void FixedUpdate()
    {
        // �ƶ�
        Move();
    }

    public virtual void Move()
    {
        rb.velocity = new Vector2(faceDir.x * currentSpeed * Time.fixedDeltaTime, rb.velocity.y);
    }

    /// <summary>
    /// ��ʱ��
    /// </summary>
    public void TimeCounter()
    {
        if (wait)
        {
            animator.SetBool("walk", false);        // ����idle״̬
            waitTimeCounter -= Time.deltaTime;
            if(waitTimeCounter <= 0)
            {
                wait = false;
                waitTimeCounter = waitTime;     // ���ü�ʱ��
                transform.localScale = new Vector3(faceDir.x, 1, 1);       // ת��
            }

        }
    }
}
