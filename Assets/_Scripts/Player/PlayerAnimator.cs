using System.Collections;
using UnityEngine;

public class PlayerAnimator : MonoBehaviour
{
    private Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    public void OnJump()
    {
        animator.SetTrigger("onJump");
    }

    public void OnRoll()
    {
        animator.SetTrigger("onRoll");
    }

    public void OnWalk(bool isWalking)
    {
        animator.SetBool("isWalk", isWalking);
    }

    public void OnRun(bool isRunning)
    {
        animator.SetBool("isRun", isRunning);
    }

    public void OnWeaponAttack()
    {
        animator.SetTrigger("onWeaponAttack");
    }

    public void OnUltimateAttack()
    {
        animator.SetTrigger("onUltimateAttack");
    }

    public void OnHit()
    {
        animator.SetTrigger("onHit");
    }

}