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


    [Header("����")]
    public VoidEventSO afterSceneLoadedEvent;

    // �����������
    public VoidEventSO cameraEventShake;


    private void Awake()
    {
        confiner2D = GetComponent<CinemachineConfiner2D>();
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
        impulseSource.GenerateImpulse();
    }

    //// �����л������
    //private void Start()
    //{
    //    GetNewCameraBounds();
    //}

    /// <summary>
    /// �Զ�ʶ��Bounds
    /// </summary>
    private void GetNewCameraBounds()
    {
        // ��ȡ��ǩΪBoudns�Ķ���
        var obj = GameObject.FindGameObjectWithTag("Bounds");
        if (obj == null)
            return;
        // ���ö����ϵ���ײ����ӵ�confider2d��
        confiner2D.m_BoundingShape2D = obj.GetComponent<Collider2D>();

        // ������
        confiner2D.InvalidateCache();
    }
}
