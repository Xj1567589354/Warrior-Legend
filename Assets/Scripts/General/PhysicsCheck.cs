using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhysicsCheck : MonoBehaviour
{
    public bool manual;
    public bool isPlayer;

    private CapsuleCollider2D coll;
    private PlayerController playerController;
    private Rigidbody2D rb;

    [Header("¼à²â×´Ì¬")]
    public bool isGround;
    public bool isTouchLeftWall;
    public bool isTouchRightWall;
    public bool onWall;

    [Header("¼à²â²ÎÊý")]
    //¼à²â°ë¾¶
    public float checkRadius;
    //¼à²âÕÚÕÖ²ã
    public LayerMask layerMask;
    //¼à²âÖÐÐÄÆ«ÒÆÁ¿
    public Vector2 bottomOffset;
    public Vector2 leftOffset;
    public Vector2 rightOffset;

    private void Awake()
    {
        coll = GetComponent<CapsuleCollider2D>();
        rb = GetComponent<Rigidbody2D>();

        // ×Ô¶¯¼ì²â×óÓÒÇ½Ìå
        if(!manual)
        {
            rightOffset = new Vector2((coll.bounds.size.x + coll.offset.x) / 2, coll.bounds.size.y / 2);
            leftOffset = new Vector2(-rightOffset.x, rightOffset.y);
        }

        if(isPlayer)
            playerController = GetComponent<PlayerController>();
    }

    private void Update()
    {
        Check();
    }

    /// <summary>
    /// »·¾³¼à²â
    /// </summary>
    public void Check()
    {
        // µØÃæ¼ì²â
        if(onWall)
            isGround = Physics2D.OverlapCircle((Vector2)transform.position+new Vector2(bottomOffset.x * transform.localScale.x, bottomOffset.y), checkRadius, layerMask);
        else
            isGround = Physics2D.OverlapCircle((Vector2)transform.position + new Vector2(bottomOffset.x * transform.localScale.x, 0), checkRadius, layerMask);

        // Ç½Ìå¼ì²â
        isTouchLeftWall = Physics2D.OverlapCircle((Vector2)transform.position + new Vector2(leftOffset.x, leftOffset.y), checkRadius, layerMask);
        isTouchRightWall = Physics2D.OverlapCircle((Vector2)transform.position + new Vector2(rightOffset.x, rightOffset.y), checkRadius, layerMask);

        // ÔÚÇ½±ÚÉÏ
        if (isPlayer)
        {
            onWall = (isTouchLeftWall && playerController.inputDirection.x < 0.0f || isTouchRightWall && playerController.inputDirection.x > 0.0f) && rb.velocity.y < 0.0f;
        }
    }


    /// <summary>
    /// »æÖÆ
    /// </summary>
    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere((Vector2)transform.position + new Vector2(bottomOffset.x * transform.localScale.x, bottomOffset.y), checkRadius);
        Gizmos.DrawWireSphere((Vector2)transform.position+new Vector2(leftOffset.x, leftOffset.y), checkRadius); 
        Gizmos.DrawWireSphere((Vector2)transform.position+ new Vector2(rightOffset.x, rightOffset.y), checkRadius); 
    }
}
