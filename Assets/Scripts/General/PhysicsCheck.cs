using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhysicsCheck : MonoBehaviour
{
    public bool manual;
    private CapsuleCollider2D coll;

    [Header("���״̬")]
    public bool isGround;
    public bool isTouchLeftWall;
    public bool isTouchRightWall;

    [Header("������")]
    //���뾶
    public float checkRadius;
    //������ֲ�
    public LayerMask layerMask;
    //�������ƫ����
    public Vector2 bottomOffset;
    public Vector2 leftOffset;
    public Vector2 rightOffset;

    private void Awake()
    {
        coll = GetComponent<CapsuleCollider2D>();

        // �Զ��������ǽ��
        if(!manual)
        {
            rightOffset = new Vector2((coll.bounds.size.x + coll.offset.x) / 2, coll.bounds.size.y / 2);
            leftOffset = new Vector2(-rightOffset.x, rightOffset.y);
        }
    }

    private void Update()
    {
        Check();
    }

    /// <summary>
    /// �������
    /// </summary>
    public void Check()
    {
        // ������
        isGround = Physics2D.OverlapCircle((Vector2)transform.position+bottomOffset, checkRadius, layerMask);

        // ǽ����
        isTouchLeftWall = Physics2D.OverlapCircle((Vector2)transform.position+leftOffset, checkRadius, layerMask);
        isTouchRightWall = Physics2D.OverlapCircle((Vector2)transform.position+rightOffset, checkRadius, layerMask);
    }


    /// <summary>
    /// ����
    /// </summary>
    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere((Vector2)transform.position+bottomOffset, checkRadius); 
        Gizmos.DrawWireSphere((Vector2)transform.position+leftOffset, checkRadius); 
        Gizmos.DrawWireSphere((Vector2)transform.position+rightOffset, checkRadius); 
    }
}
