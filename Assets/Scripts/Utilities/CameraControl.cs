using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using Unity.VisualScripting;
using UnityEngine;

public class CameraControl : MonoBehaviour
{
    [Header("���")]
    private CinemachineConfiner2D confiner2D;
    public CinemachineImpulseSource impulseSource;
    public static CameraControl instance;


    [Header("����")]
    public VoidEventSO afterSceneLoadedEvent;

    // �����������
    public VoidEventSO cameraEventShake;

    [Header("��������")]
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
        // ��
        impulseSource.GenerateImpulse();

        //// ͣ��
        //StartCoroutine(TimePause(duration));
    }

    public void StartPause()
    {
        StartCoroutine(TimePause(duration));
    }


    /// <summary>
    /// ͣ��
    /// </summary>
    /// <param name="durtation">ͣ��ʱ��</param>
    /// <returns></returns>
    IEnumerator TimePause(float durtation)
    {
        float pauseTime = durtation / 60f;      // ͣ��ʱ��
        Time.timeScale = 0;
        yield return new WaitForSecondsRealtime(pauseTime);
        Time.timeScale = 1;
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
