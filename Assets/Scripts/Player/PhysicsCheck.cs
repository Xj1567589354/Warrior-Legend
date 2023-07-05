using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhysicsCheck : MonoBehaviour
{
    [Header("���״̬")]
    public bool isGround;

    [Header("������")]
    //���뾶
    public float checkRadius;
    //������ֲ�
    public LayerMask layerMask;
    //�������ƫ����
    public Vector2 bottomOffset;
    private void Update()
    {
        Check();
    }

    /// <summary>
    /// �������
    /// </summary>
    public void Check()
    {
        isGround = Physics2D.OverlapCircle((Vector2)transform.position+bottomOffset, checkRadius, layerMask);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere((Vector2)transform.position+bottomOffset, checkRadius); 
    }
}
