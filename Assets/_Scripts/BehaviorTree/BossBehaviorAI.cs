using UnityEngine;
using System.Collections.Generic;
using System.ComponentModel.Design.Serialization;
using UnityEngine.AI;
using System.Collections;
using UnityEngine.UIElements;
public class BossBehaviorAI : MonoBehaviour
{
    [SerializeField] private float detectRange = 10f;   // 탐지 범위
    [SerializeField] private float attackRange = 5f;    // 공격 범위
    [SerializeField] private float moveSpeed = 3f;      // 이동 속도 
    [SerializeField] private float rotationSpeed = 10;  // 회전 속도

    [SerializeField] private GameObject[] breatheParticles;
    [SerializeField] private GameObject[] breatheLights;

    enum State
    {
        Sleep,
        Idle,
        Move,
        Attack,
        Jump,
        Fly,
        FlyAttack,
        Dive,
        Die
    }

    State state;
    Vector3 originPosition;
    Vector3 currentPosition;
    BehaviorTreeRunner btRunner;
    Transform detectedPlayer;
    Animator animator;
    NavMeshAgent agent;
    bool isWake = false;

    private void Start()
    {
        animator = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();

        originPosition = transform.position;
        StartSleep();

        btRunner = new BehaviorTreeRunner(SettingAttackBT()); // RootNode 세팅
    }

    private void Update()
    {
        btRunner.Operate(); // Behavior Tree 반복
    }

    // RootNode
    INode SettingAttackBT()
    {
        INode root = new SelectorNode // 원하는 액션을 실행시키기 위한 노드
        (
            // childs
            new List<INode>
            {
                new SequenceNode // 탐색 노드, Attack
                (
                    new List<INode>()
                    {
                        new ActionNode(CheckIsAttacking),       // 공격 했는지 체크
                        new ActionNode(CheckInAttackRange),       // 공격 범위 내부인지 체크
                        new ActionNode(DoAttack)                // 공격
                    }
                ),
                new SequenceNode // 탐색 노드, Chase
                (
                    new List<INode>()
                    {
                        new ActionNode(CheckDetectEnemy),     // 적 탐지
                        new ActionNode(MoveToDetectedEnemy)     // 탐지된 적에게 이동
                    }
                ),
                new ActionNode(ReturnToOrigin)                  // 원래 있던 자리로 돌아가는 액션
            }
        );

        return root; // 액션 실행
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, detectRange);
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }

    NodeState CheckIsAttacking()
    {
        // 공격 중인지 체크?
        // 보스가 가진 공격 애니메이션은 전부 추가할 필요가 있는 듯하다.
        //if(state == State.Attack || state == State.FlyAttack)
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("Attack01"))
        {
            return NodeState.Running;
        }
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("Attack02"))
        {
            return NodeState.Running;
        }
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("BreatheFire"))
        {
            return NodeState.Running;
        }
        return NodeState.Success;
    }

    NodeState CheckInAttackRange()
    {
        // 공격 범위로 들어왔는지 체크
        if (detectedPlayer != null)
        {
            if(Vector3.Magnitude(detectedPlayer.position - transform.position) < attackRange)
            {
                return NodeState.Success;
            }
        }

        return NodeState.Failure;
    }

    NodeState DoAttack()
    {
        if (detectedPlayer != null)
        {
            if(isWake == true)
            {
                // 랜덤 공격을 추가하면 될 듯 하다
                //Debug.Log(Vector3.Distance(detectedPlayer.position, transform.position));
                //if(Vector3.Distance(detectedPlayer.position, transform.position) < 3.5f)
                //{
                //    StartAttack02();
                //}
                //else
                //{
                //    StartAttack01();
                //}
                StartBreathe();
            }
            return NodeState.Success;
        }
        return NodeState.Failure;
    }

    NodeState CheckDetectEnemy()
    {
        // 스피어 콜라이더 범위 안에 들어오는 Player 레이어로 된 오브젝트 정보를 가져온다.
        // Physic.OverlapSphere(시작 위치, 반지름, 대상 레이어)
        Collider[] overlaps = Physics.OverlapSphere(transform.position, detectRange, LayerMask.GetMask("Player"));

        if (overlaps != null && overlaps.Length > 0)
        {
            detectedPlayer = overlaps[0].transform;

            if (isWake == false)
            {
                StartWakeUp();
            }

            return NodeState.Success;
        }
        detectedPlayer = null;

        return NodeState.Failure;
    }

    NodeState MoveToDetectedEnemy()
    {
        if (detectedPlayer != null)
        {
            if(Vector3.Distance(detectedPlayer.position, transform.position) < attackRange)
            {
                return NodeState.Success;
            }
            if(isWake == true)
            {
                StartMove(detectedPlayer.position);
            }
            return NodeState.Running;
        }
        return NodeState.Failure;
    }

    NodeState ReturnToOrigin()
    {
        if (Vector3.Distance(originPosition, transform.position) < 0.1)
        {
            if(isWake == true)
            {
                StartIdle();
            }
            return NodeState.Success;
        }
        else
        {
            StartMove(originPosition);
            return NodeState.Running;
        }
    }

    void StartSleep()
    {
        state = State.Sleep;
        animator.SetTrigger("Sleep");
    }

    void StartWakeUp()
    {
        StartCoroutine(BossWakeUp());
    }

    IEnumerator BossWakeUp()
    {
        yield return null;
        animator.SetTrigger("Reassemble");

        yield return new WaitForSeconds(4.3f);
        isWake = true;

    }

    void StartIdle()
    {
        state = State.Idle;
        animator.SetTrigger("Idle");
    }

    void StartMove(Vector3 position)
    {
        state = State.Move;
        Vector3 direction = position - transform.position;
        if(direction.magnitude > moveSpeed *Time.deltaTime)
        {
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            transform.rotation =
                Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }
        animator.SetTrigger("Walk");
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("Walk"))
        {
            transform.position = Vector3.MoveTowards(transform.position, position, moveSpeed * Time.deltaTime);
        }
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("Run"))
        {
            transform.position = Vector3.MoveTowards(transform.position, position, (moveSpeed + 3) * Time.deltaTime);
        }
    }

    void StartAttack01()
    {
        state = State.Attack;
        animator.SetTrigger("Attack01");
    }

    void StartAttack02()
    {
        state = State.Attack;
        animator.SetTrigger("Attack02");
    }

    void StartTailAttack()
    {
        state = State.Attack;
        animator.SetTrigger("TailWhipL");
    }

    void StartBreathe()
    {
        state = State.Attack;
        animator.SetTrigger("BreatheFire");
        Invoke("StartBreatheParticle", 0.3f);
        Invoke("StopBreatheParticle", 2.5f);
    }

    void StartJump()
    {
        state = State.Jump;
        animator.SetTrigger("Jump");
    }

    void StartFly()
    {
        state = State.Fly;
        animator.SetTrigger("FlyIdle");
    }

    void StartFlyAttack()
    {
        state = State.FlyAttack;
        animator.SetTrigger("FlyAttack");
    }

    void StartFlyBreathe()
    {
        state = State.FlyAttack;
        animator.SetTrigger("FlyBreatheFire");
        Invoke("StartBreatheParticle", 0.3f);
        Invoke("StopBreatheParticle", 2.5f);
    }

    void StartDive()
    {
        state = State.Dive;
        animator.SetTrigger("FlyDive");
    }

    // Dragon Breathe
    public void StartBreatheParticle()
    {
        for(int l =0; l< breatheLights.Length; l++)
        {
            breatheLights[l].SetActive(true);
        }
        for(int p = 0; p < breatheParticles.Length; p++)
        {
            breatheParticles[p].GetComponent<ParticleSystem>().Play();
        }
    }

    public void StopBreatheParticle()
    {
        for (int l = 0; l < breatheLights.Length; l++)
        {
            breatheLights[l].SetActive(false);
        }
        for (int p = 0; p < breatheParticles.Length; p++)
        {
            breatheParticles[p].GetComponent<ParticleSystem>().Stop();
        }
    }
}
