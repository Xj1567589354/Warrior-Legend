using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// 实现打击感
/// </summary>
public class AttackPause : MonoBehaviour
{
    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Enemy"))
        {
            CameraControl.instance.StartPause();
        }
    }
}
