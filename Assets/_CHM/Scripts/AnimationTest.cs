using UnityEngine;

public class AnimationTest : MonoBehaviour
{
    Animator animator;

    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    public void PlayIdle()
    {
        animator.SetTrigger("Idle");
    }
    public void PlayIdleBreak()
    {
        animator.SetTrigger("IdleBreak");
    }
    public void PlayWalk()
    {
        animator.SetTrigger("Walk");
    }
    public void PlayRun()
    {
        animator.SetTrigger("Run");
    }
    public void PlayDeath()
    {
        animator.SetTrigger("Death");
    }
    public void PlayReassemble()
    {
        animator.SetTrigger("Reassemble");
    }
    public void PlayAttack01()
    {
        animator.SetTrigger("Attack01");
    }
    public void PlayAttack02()
    {
        animator.SetTrigger("Attack02");
    }
    public void PlayTailWhipL()
    {
        animator.SetTrigger("TailWhipLeft");
    }
    public void PlayBreatheFire()
    {
        animator.SetTrigger("BreatheFire");
    }
    public void PlayFlyAttack()
    {
        animator.SetTrigger("FlyAttackSet");
    }
    public void PlayFlyBreatheFire()
    {
        animator.SetTrigger("FlyBreatheFireSet");
    }
}
