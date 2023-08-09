using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using Unity.VisualScripting;
using UnityEngine;

public class CameraControl : MonoBehaviour
{
    private CinemachineConfiner2D confiner2D;
    public CinemachineImpulseSource impulseSource;

    public VoidEventSO cameraEventShake;


    private void Awake()
    {
        confiner2D = GetComponent<CinemachineConfiner2D>();
    }

    private void OnEnable()
    {
        cameraEventShake.onEventRaised += OnCameraShakeEvent;
    }

    private void OnDisable()
    {
        cameraEventShake.onEventRaised -= OnCameraShakeEvent;
    }

    private void OnCameraShakeEvent()
    {
        impulseSource.GenerateImpulse();
    }

    // 场景切换后更改
    private void Start()
    {
        GetNewCameraBounds();
    }

    /// <summary>
    /// 自动识别Bounds
    /// </summary>
    private void GetNewCameraBounds()
    {
        // 获取标签为Boudns的对象
        var obj = GameObject.FindGameObjectWithTag("Bounds");
        if (obj == null)
            return;
        // 将改对象上的碰撞体添加到confider2d上
        confiner2D.m_BoundingShape2D = obj.GetComponent<Collider2D>();

        // 清理缓存
        confiner2D.InvalidateCache();
    }
}
