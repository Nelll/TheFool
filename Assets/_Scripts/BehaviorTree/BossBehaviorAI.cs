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

    enum State
    {
        Idle,
        Move,
        Attack,
        Die
    }

    State state;
    Vector3 originPosition;
    Vector3 currentPosition;
    BehaviorTreeRunner btRunner;
    Transform detectedPlayer;
    Animator animator;
    NavMeshAgent agent;
    bool isWake;

    private void Start()
    {
        animator = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();

        originPosition = transform.position;
        state = State.Idle;

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
        if(animator.GetCurrentAnimatorStateInfo(0).IsName("Attack01"))
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
            // 랜덤 공격을 추가하면 될 듯 하다
            StartAttack01();
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

            //if(isWake == false)
            //{
            //    StartWakeUp();
            //}

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
            StartMove(detectedPlayer.position);
            return NodeState.Running;
        }
        return NodeState.Failure;
    }

    NodeState ReturnToOrigin()
    {
        if (Vector3.Distance(originPosition, transform.position) < 0.1)
        {
            StartIdle();
            return NodeState.Success;
        }
        else
        {
            StartMove(originPosition);
            return NodeState.Running;
        }
    }

    void StartWakeUp()
    {
        isWake = true;
        StartCoroutine(BossWakeUp());
    }

    IEnumerator BossWakeUp()
    {
        animator.SetTrigger("Reassemble");
        yield return new WaitForSeconds(1);
    }

    void StartIdle()
    {
        state = State.Idle;
        //agent.isStopped = true;
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
        if (Vector3.Distance(originPosition, transform.position) < 0.1)
        {
            transform.Rotate(new Vector3(0, 0, 0));
        }
        transform.position = Vector3.MoveTowards(transform.position, position, moveSpeed * Time.deltaTime);
        //agent.destination = position;
        //agent.isStopped = false;
        //animator.SetTrigger("Run");
    }

    void StartAttack01()
    {
        state = State.Attack;
        //agent.isStopped = false;
        animator.SetTrigger("Attack01");
    }
}
