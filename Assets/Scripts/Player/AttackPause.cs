using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// ʵ�ִ����
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
