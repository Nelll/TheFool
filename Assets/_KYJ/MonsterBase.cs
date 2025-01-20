using UnityEngine;
using UnityEngine.AI;
using System.Collections;

public enum MonsterState { Wander, Chase, Attack, Death }

public abstract class MonsterBase : MonoBehaviour
{
    #region Variables
    public MonsterState monsterState;
    [SerializeField] protected Transform target;
    [SerializeField] float distanceToPlayer;
    [SerializeField] protected float chaseRange;
    [SerializeField] protected float attackRange;

    protected NavMeshAgent agent;
    protected Animator animator;

    protected bool isAttacking = false;
    bool isDead = false;
    bool isRotate = false;

    [SerializeField] protected Transform[] waypoints;
    protected int curretWaypointIndex = 0;

    float attackCooldown = 3.0f;
    float attackCooldownTimer = 0;
    #endregion

    #region Methods
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
        SetCooldown();
        StateMachine();
    }

    protected abstract void Wander();

    protected virtual void Chase()
    {
        if (target == null)
        {
            Debug.LogError($"{transform.name}�� Chase �Լ� ���� �߻�.\nŸ�� �÷��̾ �������� �ʾҽ��ϴ�.");
            return;
        }

        MoveToTarget();
        MoveBlendTree();
    }

    protected abstract IEnumerator Attack();

    protected virtual void Death()
    {
        isDead = true;
        agent.isStopped = true;
        animator.SetTrigger("Death");
        StartCoroutine(DestroyObject(5.0f));
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
                if (attackCooldownTimer <= 0)
                {
                    StartCoroutine(LookAtPlayer());
                    StartCoroutine(Attack());
                    attackCooldownTimer = attackCooldown;
                }
                break;

            case MonsterState.Death:
                Death();
                break;
        }
    }
    #endregion

    #region Functions
    IEnumerator DestroyObject(float destroyTime) // Death ȣ��� ������Ʈ �ı�
    {
        yield return new WaitForSeconds(destroyTime);
        Destroy(gameObject);
    }

    void DistanceToPlayer() // �÷��̾� ���� �Ÿ�
    {
        if (target != null)
        {
            distanceToPlayer = Vector3.Distance(transform.position, target.position);
        }
    }

    void CheckToRange(float rangeType, MonsterState wantState) // �Ÿ��� ���� ���� ��ȭ
    {
        if (distanceToPlayer <= rangeType)
        {
            monsterState = wantState;
        }
    }

    void CheckToRange(float rangeType, MonsterState wantState, float fallBackRangeType, MonsterState fallBackState) // �Ÿ��� ���� ���� ��ȭ
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

    void FallBackState(float rangeType, MonsterState wantState) // ���� ���·� ���ư���
    {
        if (distanceToPlayer > rangeType)
        {
            monsterState = wantState;
        }
    }

    protected IEnumerator LookAtPlayer() // �÷��̾� �ٶ󺸱�
    {
        if (isRotate || target == null) yield break;
        isRotate = true;

        Vector3 direction = (target.position - transform.position).normalized;
        direction.y = 0;
        Quaternion lookRotation = Quaternion.LookRotation(direction);
        float angle = Quaternion.Angle(transform.rotation, lookRotation);

        while (angle > 1f)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f);
            yield return null;
            angle = Quaternion.Angle(transform.rotation, lookRotation);
        }
        isRotate = false;
    }

    protected void MoveToTarget() // �÷��̾� ����
    {
        if (target == null)
        {
            Debug.LogError($"{transform.name}�� MoveToTarget �Լ� ���� �߻�.\nŸ�� �÷��̾ �������� �ʾҽ��ϴ�.");
            return;
        }

        agent.SetDestination(target.position);
    }

    protected void MoveToWaypoint() // ��������Ʈ�� �̵�
    {
        if (waypoints.Length == 0)
        {
            Debug.LogError($"{transform.name}�� MoveToWaypoint �Լ� ���� �߻�.\n��������Ʈ �迭�� ����ֽ��ϴ�.");
            return;
        }

        agent.SetDestination(waypoints[curretWaypointIndex].position);
    }

    protected void MoveBlendTree() // �̵� ����Ʈ�� ����
    {
        float targetMagnitude = agent.velocity.magnitude > 0.1f ? 1f : 0f;
        float currentMagnitude = animator.GetFloat("SpeedMagnitude"); 
        float smoothMagnitude = Mathf.Lerp(currentMagnitude, targetMagnitude, Time.deltaTime * 1.5f);
        animator.SetFloat("SpeedMagnitude", smoothMagnitude);
    }

    void SetCooldown() // ���� ��Ÿ��
    {
        if (attackCooldown > 0)
        {
            attackCooldownTimer -= Time.deltaTime;
        }
    }
    #endregion
}