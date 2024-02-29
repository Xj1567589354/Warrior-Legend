using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.DualShock;

public class Sign : MonoBehaviour
{
    private Animator anim;
    public GameObject signSprite;
    public Transform playerTrans;
    public bool canPress;
    private PlayerInputControl playerInput;
    private Iinteractable targetItem;       // ��������

    private void Awake()
    {
        anim = signSprite.GetComponent<Animator>();
        
        // ��������ϵͳ
        playerInput = new PlayerInputControl();
        playerInput.Enable();
    }

    private void OnEnable()
    {
        InputSystem.onActionChange += OnActionChange;
        playerInput.Gameplay.Confirm.started += OnConfirm;
    }

    private void OnDisable()
    {
        canPress = false;
    }

    private void Update()
    {
        signSprite.GetComponent<SpriteRenderer>().enabled = canPress;
        signSprite.transform.localScale = playerTrans.localScale;
    }

    /// <summary>
    /// ��������
    /// </summary>
    /// <param name="obj"></param>
    /// <param name="actionChange"></param>
    private void OnActionChange(object obj, InputActionChange actionChange)
    {   
        // �жϲ������Ƿ����
        if(actionChange == InputActionChange.ActionStarted)
        {
            // ��ӡ������
            //Debug.Log(((InputAction)obj).activeControl.device);

            // ����������
            var d = ((InputAction)obj).activeControl.device;
            // ���ݲ�ͬ���������Ʋ��Ŷ�Ӧ����
            switch (d.device) 
            {
                case Keyboard:                      // ����
                    anim.Play("Keyboard");
                    break;
                case DualShockGamepad:              // �ֱ�
                    anim.Play("Ps");
                    break;
            }
        }
    }

    private void OnConfirm(InputAction.CallbackContext obj)
    {
        if (canPress)
        {
            targetItem.TriggerAction();
            GetComponent<AudioDefination>()?.PlayAudioClip();        // ������Ч
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Interactable"))
        {
            canPress = true;
            targetItem = other.GetComponent<Iinteractable>();       // ��ȡ��ײ����Ľӿ�������
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        canPress = false;
    }
}
