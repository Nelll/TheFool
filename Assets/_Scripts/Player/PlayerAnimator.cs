using System.Collections;
using UnityEngine;

public class PlayerAnimator : MonoBehaviour
{
    [SerializeField]
    private GameObject attackCollision;
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

    //public void OnHit()
    //{
    //    animator.SetBool("isHit", isHitting);
    //}

    public void OnWeaponAttack()
    {
        animator.SetTrigger("onWeaponAttack");
    }

    // 애니메이션이 재생되는 도중 특정 프레임에서 호출 하도록 설정
    public void OnAttackCollision()
    {
        attackCollision.SetActive(true);
    }
}
