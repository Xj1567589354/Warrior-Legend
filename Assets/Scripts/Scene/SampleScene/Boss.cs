using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : MonoBehaviour
{
    public GameObject boss;
    public GameObject bossSavePoint;
    private void OnEnable()
    {
        if (DataManager.instance.isBoss)
        {
            boss.SetActive(true);
            bossSavePoint.SetActive(true);
        }
    }
}
