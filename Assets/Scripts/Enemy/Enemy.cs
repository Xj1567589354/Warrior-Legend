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

    [Header("��������")]
    public float normalSpeed;   // �ƶ��ٶ�
    public float chaseSpeed;    // ׷���ٶ�
    public float currentSpeed;  // ��ǰ�ٶ�
    public float hurtForce;     // ������
    public Vector3 faceDir;     // �泯����
    public Vector2 hurtDir;     // ���˷���

    private Transform attacker;  // ������

    [Header("��ʱ��")]
    public float waitTime;      // �ȴ�ʱ��
    public float waitTimeCounter;   // ��ʱ��
    public bool wait;       // �ȴ�״̬

    [Header("״̬")]
    public bool isHurt;     // ����
    public bool isDead;     // ����

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
        if(!isHurt && !isDead)
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

     public void OnTakeDamage(Transform attackerTrans)
    {
        // ���
        attacker = attackerTrans;    
        // ת��
        if (attacker.position.x - transform.position.x > 0)
            transform.localScale = new Vector3(-1, 1, 1);
        if (attacker.position.x - transform.position.x < 0)
            transform.localScale = new Vector3(1, 1, 1);

        // ���˻���
        isHurt = true;
        animator.SetTrigger("hurt");
        hurtDir = new Vector2(transform.position.x - attacker.position.x, 0).normalized;

        StartCoroutine(OnHurt(hurtDir));
    }
    
    /// <summary>
    /// Э�̷���
    /// </summary>
    /// <param name="_hurtDir">���˷���</param>
    /// <returns></returns>
    private IEnumerator OnHurt(Vector2 _hurtDir)
    {
        rb.AddForce(_hurtDir * hurtForce, ForceMode2D.Impulse);
        // �ȴ�0.45s֮���������״̬
        yield return new WaitForSeconds(0.5f);    
        isHurt = false;
    }

    /// <summary>
    /// ����
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
