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
    private Iinteractable targetItem;       // 互动物体

    private void Awake()
    {
        anim = signSprite.GetComponent<Animator>();
        
        // 启动控制系统
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
    /// 检测操作器
    /// </summary>
    /// <param name="obj"></param>
    /// <param name="actionChange"></param>
    private void OnActionChange(object obj, InputActionChange actionChange)
    {   
        // 判断操作器是否更改
        if(actionChange == InputActionChange.ActionStarted)
        {
            // 打印操作器
            //Debug.Log(((InputAction)obj).activeControl.device);

            // 操作器名称
            var d = ((InputAction)obj).activeControl.device;
            // 根据不同操作器名称播放对应动画
            switch (d.device) 
            {
                case Keyboard:                      // 键盘
                    anim.Play("Keyboard");
                    break;
                case DualShockGamepad:              // 手柄
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
            GetComponent<AudioDefination>()?.PlayAudioClip();        // 播放音效
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Interactable"))
        {
            canPress = true;
            targetItem = other.GetComponent<Iinteractable>();       // 获取碰撞物体的接口子物体
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        canPress = false;
    }
}
