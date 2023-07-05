using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// 泛型单例模式
/// </summary>
/// <typeparam name="T">继承类</typeparam>
public class Singleton<T> : MonoBehaviour where T : Singleton<T>
{
    //定义一个泛型类型静态私有变量instance
    private static T instance;
    //创建一个公有静态变量Instance，返回instance
    public static T Instance
    {
        get { return instance; }
    }

    //virtual--可以在子类中override
    protected virtual void Awake()
    {
        if (instance == null)
            Destroy(gameObject);
        else
            //因为是不同类继承，所以要加泛型T
            instance = (T)this;
    }
}
