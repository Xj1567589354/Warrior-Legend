using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhysicsCheck : MonoBehaviour
{
    [Header("¼à²â×´Ì¬")]
    public bool isGround;

    [Header("¼à²â²ÎÊý")]
    //¼à²â°ë¾¶
    public float checkRadius;
    //¼à²âÕÚÕÖ²ã
    public LayerMask layerMask;
    //¼à²âÖÐÐÄÆ«ÒÆÁ¿
    public Vector2 bottomOffset;
    private void Update()
    {
        Check();
    }

    /// <summary>
    /// »·¾³¼à²â
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
