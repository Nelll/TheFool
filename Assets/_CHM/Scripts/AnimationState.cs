using UnityEngine;

enum State
{
    Idle,
    Walk,
    Run,
    Jump,
    Fall,
    Landing,
    Attack,
    Die,
}

public class AnimationState : MonoBehaviour
{

    State state;
    Animator animator;
    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    public void IdleAction()
    {
        SetState(State.Idle);
    }

    public void WalkAction()
    {
        SetState(State.Walk);
    }

    public void RunAction()
    {
        SetState(State.Run);
    }

    public void AttackAction()
    {
        SetState(State.Attack);
    }

    public void JumpAction()
    {
        SetState(State.Jump);
    }

    public void FallAction()
    {
        SetState(State.Fall);
    }

    public void LandingAction()
    {
        SetState(State.Landing);
    }

    void SetState(State newState)
    {
        switch (state)
        {
            case State.Idle:
                if(newState == State.Attack)
                {
                    animator.SetTrigger("Attack");
                }
                else if (newState == State.Walk)
                {
                    animator.SetTrigger("Walk");
                }
                else if (newState == State.Run)
                {
                    animator.SetTrigger("Run");
                }
                else if(newState == State.Jump)
                {
                    animator.SetTrigger("Jump");
                }
                else if(newState == State.Fall)
                {
                    animator.SetTrigger("Fall");
                }
                break;
            case State.Walk:
                if(newState == State.Attack)
                {
                    animator.SetTrigger("Attack");
                }
                else if(newState == State.Run)
                {
                    animator.SetTrigger("Run");
                }
                else if(newState == State.Idle)
                {
                    animator.SetTrigger("Idle");
                }
                else if(newState == State.Jump)
                {
                    animator.SetTrigger("Jump");
                }
                else if (newState == State.Fall)
                {
                    animator.SetTrigger("Fall");
                }
                break;
            case State.Run:
                if (newState == State.Attack)
                {
                    animator.SetTrigger("Attack");
                }
                else if (newState == State.Walk)
                {
                    animator.SetTrigger("Walk");
                }
                else if (newState == State.Idle)
                {
                    animator.SetTrigger("Idle");
                }
                else if (newState == State.Jump)
                {
                    animator.SetTrigger("Jump");
                }
                else if (newState == State.Fall)
                {
                    animator.SetTrigger("Fall");
                }
                break;
            case State.Attack:
                if (newState == State.Idle)
                {
                    animator.SetTrigger("Idle");
                }
                else if (newState == State.Walk)
                {
                    animator.SetTrigger("Walk");
                }
                else if (newState == State.Run)
                {
                    animator.SetTrigger("Run");
                }
                else if (newState == State.Jump)
                {
                    animator.SetTrigger("Jump");
                }
                else if (newState == State.Fall)
                {
                    animator.SetTrigger("Fall");
                }
                break;
            case State.Jump:
                if (newState == State.Landing)
                {
                    animator.SetTrigger("Landing");
                }
                break;
            case State.Fall:
                if (newState == State.Landing)
                {
                    animator.SetTrigger("Landing");
                }
                break;
            case State.Landing:
                if(newState == State.Idle)
                {
                    animator.SetTrigger("Idle");
                }
                else if(newState == State.Attack)
                {
                    animator.SetTrigger("Attack");
                }
                else if(newState == State.Walk)
                {
                    animator.SetTrigger("Walk");
                }
                else if (newState == State.Run)
                {
                    animator.SetTrigger("Run");
                }
                else if (newState == State.Jump)
                {
                    animator.SetTrigger("Jump");
                }
                break;
        }

        state = newState;
    }
}
