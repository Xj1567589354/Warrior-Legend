using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using Unity.VisualScripting;
using UnityEngine;

public class CameraControl : MonoBehaviour
{
    [Header("组件")]
    private CinemachineConfiner2D confiner2D;
    public CinemachineImpulseSource impulseSource;
    public static CameraControl instance;


    [Header("监听")]
    public VoidEventSO afterSceneLoadedEvent;

    // 监听摄像机振动
    public VoidEventSO cameraEventShake;

    [Header("基础参数")]
    public float duration;
    


    private void Awake()
    {
        confiner2D = GetComponent<CinemachineConfiner2D>();
        
        if(instance == null )
            instance = this;
        else
            Destroy( this.gameObject );
    }

    private void OnEnable()
    {
        cameraEventShake.onEventRaised += OnCameraShakeEvent;
        afterSceneLoadedEvent.onEventRaised += OnAfterSceneLoaderEvent;
    }

    private void OnDisable()
    {
        cameraEventShake.onEventRaised -= OnCameraShakeEvent;
        afterSceneLoadedEvent.onEventRaised -= OnAfterSceneLoaderEvent;
    }

    private void OnAfterSceneLoaderEvent()
    {
        GetNewCameraBounds();
    }

    private void OnCameraShakeEvent()
    {
        // 震动
        impulseSource.GenerateImpulse();

        //// 停顿
        //StartCoroutine(TimePause(duration));
    }

    public void StartPause()
    {
        StartCoroutine(TimePause(duration));
    }


    /// <summary>
    /// 停顿
    /// </summary>
    /// <param name="durtation">停顿时间</param>
    /// <returns></returns>
    IEnumerator TimePause(float durtation)
    {
        float pauseTime = durtation / 60f;      // 停顿时间
        Time.timeScale = 0;
        yield return new WaitForSecondsRealtime(pauseTime);
        Time.timeScale = 1;
    }

    //// 场景切换后更改
    //private void Start()
    //{
    //    GetNewCameraBounds();
    //}

    /// <summary>
    /// 自动识别Bounds
    /// </summary>
    private void GetNewCameraBounds()
    {
        // 获取标签为Boudns的对象
        var obj = GameObject.FindGameObjectWithTag("Bounds");
        if (obj == null)
            return;
        // 将该对象上的碰撞体添加到confider2d上
        confiner2D.m_BoundingShape2D = obj.GetComponent<Collider2D>();

        // 清理缓存
        confiner2D.InvalidateCache();
    }
}
