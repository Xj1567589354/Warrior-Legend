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

    [Header("��������")]
    public float normalSpeed;   // �ƶ��ٶ�
    public float chaseSpeed;    // ׷���ٶ�
    [HideInInspector] public float currentSpeed;  // ��ǰ�ٶ�
    public float hurtForce;     // ������
    [HideInInspector] public Vector3 faceDir;     // �泯����
    [HideInInspector] public Vector2 hurtDir;     // ���˷���

    public Transform attacker;  // ������

    [Header("��ʱ��")]
    public float waitTime;      // �ȴ�ʱ��
    public float waitTimeCounter;   // �ȴ���ʱ��
    public bool wait;       // �ȴ�״̬
    public float lostTime;  // ��ʧʱ��
    public float lostTimeCounter;   // ��ʧ��ʱ��

    [Header("״̬")]
    public bool isHurt;     // ����
    public bool isDead;     // ����

    [Header("����״̬")]
    private BaseState currentState;
    protected BaseState patrolState;        // Ѳ��״̬
    protected BaseState chaseState;         // ׷��״̬
    protected BaseState skillState;         // ���״̬

    [Header("���")]
    public Vector2 centerOffset;    // ���ƫ��
    public Vector2 checkSize;       // ����С
    public float checkDistance;     // ������
    public LayerMask attackLayer;   // ͼ��
            
    protected virtual void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();

        currentSpeed = normalSpeed;
        physicsCheck = GetComponent<PhysicsCheck>();

        // ��ʼ����ʱ��
        waitTimeCounter = waitTime;
    }

    private void OnEnable()
    {
        currentState = patrolState;
        currentState.OnEnter(this);
    }

    private void Update()
    {
        // ��ȡ����
        faceDir = new Vector3(-transform.localScale.x, 0, 0);

        currentState.LogicUpdate();
        TimeCounter();      // ��ʱ��
    }

    private void FixedUpdate()
    {
        currentState.PhysicsUpdate();
        if (!isHurt && !isDead && !wait)
            // �ƶ�
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
    /// ��ʱ��
    /// </summary>
    public void TimeCounter()
    {
        if (wait)
        {
            waitTimeCounter -= Time.deltaTime;
            if(waitTimeCounter <= 0)
            {
                wait = false;
                waitTimeCounter = waitTime;     // ���ü�ʱ��
                transform.localScale = new Vector3(faceDir.x, 1, 1);       // ת��
            }
        }

        // ��ʧ��ʱ����
        if (!FoundPlayer() && lostTimeCounter > 0)
            lostTimeCounter -= Time.deltaTime;
        else if (FoundPlayer())
            lostTimeCounter = lostTime;
    }

    /// <summary>
    /// ���ֵ���
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
    /// ת��״̬
    /// </summary>
    /// <param name="state">Ŀ��״̬</param>
    public void SwitchState(NPCState state)
    {
        var newState = state switch
        {
            NPCState.Patrol => patrolState,
            NPCState.Chase => chaseState,
            NPCState.Skill => skillState,
            _=>null
        };
        currentState.OnExit();          // �˳���һ״̬
        currentState = newState;        // ������״̬
        currentState.OnEnter(this);     // ������״̬
    }

    #region �¼�ִ�з���
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
        /*Ұ������ʱ��x���ϵ���ͣ��������ֹҰ��ԭ�����˶���*/
        rb.velocity = new Vector2(0, rb.velocity.y);
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
    #endregion
}
