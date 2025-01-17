using UnityEngine;
using UnityEngine.AI;
using System.Collections;

public enum MonsterState { Wander, Chase, Attack, Death }

public abstract class MonsterBase : MonoBehaviour
{
    public MonsterState monsterState;
    [SerializeField] protected Transform target;
    [SerializeField] float distanceToPlayer; // �÷��̾���� �Ÿ�
    [SerializeField] protected float chaseRange; // ���� �ൿ�� ������ ����
    [SerializeField] protected float attackRange; // ���� �ൿ�� ������ ����
    protected Animator animator;
    protected bool isAttacking = false;
    bool isDead = false;
    public Transform[] waypoints;
    protected NavMeshAgent agent;

    void Start()
    {
        animator = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
        target = GameObject.FindWithTag("Player")?.transform;

        if (target == null)
        {
            Debug.LogError("MonsterBase.cs���� Target ������ �����߽��ϴ�. ��ũ��Ʈ�� ��Ȱ��ȭ �˴ϴ�");
            enabled = false;
        }
    }

    protected virtual void Update()
    {
        if (isDead) return;

        DistanceToPlayer();
        StateMachine();
    }
        
    protected abstract void Wander();
    protected abstract void Chase();
    protected abstract IEnumerator Attack();
    protected virtual void Death()
    {
        isDead = true;
        StartCoroutine(DestroyObject(5.0f)); // 5�� �� ������Ʈ ����
    }

    void StateMachine()
    {
        switch (monsterState)
        {
            case MonsterState.Wander:
                /// <summary>
                /// ���� ���� ���̸� ���� ���·�
                /// </summary>
                CheckToRange(chaseRange, MonsterState.Chase);
                Wander();
                break;

            case MonsterState.Chase:
                /// <summary> 
                /// ���� ���� ���̸� ���� ���·�,
                /// ���� ���� ���̸� ��ȸ ���·�
                /// </summary>
                CheckToRange(attackRange, MonsterState.Attack, chaseRange, MonsterState.Wander);
                Chase();
                break;

            case MonsterState.Attack:
                /// <summary>
                /// ���� ���� ���̸� ���� ���·�
                /// </summary>
                FallBackState(attackRange, MonsterState.Chase);
                StartCoroutine(Attack());
                break;

            case MonsterState.Death:
                Death();
                break;
        }
    }

    IEnumerator DestroyObject(float destroyTime)
    {
        yield return new WaitForSeconds(destroyTime);
        Destroy(gameObject);
    }

    void DistanceToPlayer()
    {
        if (target != null)
        {
            distanceToPlayer = Vector3.Distance(transform.position, target.position);
        }
    }

    void CheckToRange(float rangeType, MonsterState wantState)
    {
        if (distanceToPlayer <= rangeType)
        {
            monsterState = wantState;
        }
    }

    void CheckToRange(float rangeType, MonsterState wantState, float fallBackRangeType, MonsterState fallBackState)
    {
        if (distanceToPlayer <= rangeType)
        {
            monsterState = wantState;
        }
        else if (distanceToPlayer > fallBackRangeType)
        {
            monsterState = fallBackState;
        }
    }

    void FallBackState(float rangeType, MonsterState wantState)
    {
        if (distanceToPlayer > rangeType)
        {
            monsterState = wantState;
        }
    }
}
