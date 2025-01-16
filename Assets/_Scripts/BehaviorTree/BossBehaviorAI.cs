using UnityEngine;
using System.Collections.Generic;
using System.ComponentModel.Design.Serialization;
using UnityEngine.AI;
using System.Collections;
using UnityEngine.UIElements;
public class BossBehaviorAI : MonoBehaviour
{
    [SerializeField] private float detectRange = 10f;   // Ž�� ����
    [SerializeField] private float attackRange = 5f;    // ���� ����
    [SerializeField] private float moveSpeed = 3f;      // �̵� �ӵ� 
    [SerializeField] private float rotationSpeed = 10;  // ȸ�� �ӵ�

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

        btRunner = new BehaviorTreeRunner(SettingAttackBT()); // RootNode ����
    }

    private void Update()
    {
        btRunner.Operate(); // Behavior Tree �ݺ�
    }

    // RootNode
    INode SettingAttackBT()
    {
        INode root = new SelectorNode // ���ϴ� �׼��� �����Ű�� ���� ���
        (
            // childs
            new List<INode>
            {
                new SequenceNode // Ž�� ���, Attack
                (
                    new List<INode>()
                    {
                        new ActionNode(CheckIsAttacking),       // ���� �ߴ��� üũ
                        new ActionNode(CheckInAttackRange),       // ���� ���� �������� üũ
                        new ActionNode(DoAttack)                // ����
                    }
                ),
                new SequenceNode // Ž�� ���, Chase
                (
                    new List<INode>()
                    {
                        new ActionNode(CheckDetectEnemy),     // �� Ž��
                        new ActionNode(MoveToDetectedEnemy)     // Ž���� ������ �̵�
                    }
                ),
                new ActionNode(ReturnToOrigin)                  // ���� �ִ� �ڸ��� ���ư��� �׼�
            }
        );

        return root; // �׼� ����
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
        // ���� ������ üũ?
        // ������ ���� ���� �ִϸ��̼��� ���� �߰��� �ʿ䰡 �ִ� ���ϴ�.
        if(animator.GetCurrentAnimatorStateInfo(0).IsName("Attack01"))
        {
            return NodeState.Running;
        }
        return NodeState.Success;
    }

    NodeState CheckInAttackRange()
    {
        // ���� ������ ���Դ��� üũ
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
            // ���� ������ �߰��ϸ� �� �� �ϴ�
            StartAttack01();
            return NodeState.Success;
        }
        return NodeState.Failure;
    }

    NodeState CheckDetectEnemy()
    {
        // ���Ǿ� �ݶ��̴� ���� �ȿ� ������ Player ���̾�� �� ������Ʈ ������ �����´�.
        // Physic.OverlapSphere(���� ��ġ, ������, ��� ���̾�)
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
