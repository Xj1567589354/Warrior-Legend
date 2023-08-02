using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    private PlayerController controller;
    private PhysicsCheck physicsCheck;

    private Animator animator;
    private Rigidbody2D rb;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        controller = GetComponent<PlayerController>();
        physicsCheck = GetComponent<PhysicsCheck>();
    }

    private void Update()
    {
        SetAnimation();
    }

    void SetAnimation()
    {
        animator.SetFloat("velocityX", Mathf.Abs(rb.velocity.x));
        animator.SetFloat("velocityY", rb.velocity.y);
        animator.SetBool("run", controller.isRun);
        animator.SetBool("isGround", physicsCheck.isGround);
        animator.SetBool("isCrouch", controller.isCrouch);
        animator.SetBool("isDead", controller.isDead);
        animator.SetBool("isAttack", controller.isAttack);
        animator.SetBool("onWall", physicsCheck.onWall);
        animator.SetBool("isSlide", controller.isSlide);
    }

    /// <summary>
    /// 执行受伤动画
    /// </summary>
    public void PlayerHurt()
    {
        animator.SetTrigger("hurt");
    }

    /// <summary>
    /// 执行攻击动画
    /// </summary>
    public void PlayAttack()
    {
        animator.SetTrigger("attack");
    }
}
