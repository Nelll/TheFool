using UnityEngine;

public class TestMonsterKYJ : MonoBehaviour
{
    Animator anim;

    void Start() => anim = GetComponent<Animator>();

    public void AnimationIdle() => anim.SetTrigger("Idle");
    public void AnimationWalk() => anim.SetTrigger("Walk");
    public void AnimationAttack() => anim.SetTrigger("Attack");
    public void AnimationPunch() => anim.SetTrigger("Punch");
    public void AnimationDeath() => anim.SetTrigger("Death");
    public void PositionReset() => transform.SetPositionAndRotation(Vector3.zero, Quaternion.Euler(Vector3.zero));
    public void ToggleAnimation() => anim.enabled = !anim.enabled;
}