using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// ���͵���ģʽ
/// </summary>
/// <typeparam name="T">�̳���</typeparam>
public class Singleton<T> : MonoBehaviour where T : Singleton<T>
{
    //����һ���������;�̬˽�б���instance
    private static T instance;
    //����һ�����о�̬����Instance������instance
    public static T Instance
    {
        get { return instance; }
    }

    //virtual--������������override
    protected virtual void Awake()
    {
        if (instance == null)
            Destroy(gameObject);
        else
            //��Ϊ�ǲ�ͬ��̳У�����Ҫ�ӷ���T
            instance = (T)this;
    }
}
