using UnityEngine;
using UnityEngine.AI;
using System.Collections;

public enum MonsterState { Wander, Chase, Attack, Death }

public abstract class MonsterBase : MonoBehaviour
{
    #region Variables
    public MonsterState monsterState;
    public MonsterState MonsterState { set { monsterState = value; } }

    [SerializeField] protected Transform target;
    [SerializeField] protected float chaseRange;
    [SerializeField] protected float attackRange;
    [SerializeField] protected float moveRadius;
    [SerializeField] LayerMask obstaclesLayer; // ��ֹ��� ������ ���̾�
    [SerializeField] float distanceToPlayer;
    [HideInInspector] public float ChaseRange { get { return chaseRange; } }
    [HideInInspector] public float AttackRange { get { return attackRange; } }
    [HideInInspector] public float MoveRadius { get { return moveRadius; } }
    protected NavMeshAgent agent;
    protected Animator animator;
    protected bool isAttacking = false;
    protected bool isDead = false;
    bool isRotate = false;
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
        DistanceToPlayer();
        SetCooldown();
        StateMachine();
    }
    #endregion

    #region StateMachine
    void StateMachine()
    {
        switch (monsterState)
        {
            case MonsterState.Wander:
                CheckToRange(chaseRange, MonsterState.Chase); // ���� ���� ���̸� ���� ����
                Wander();
                break;

            case MonsterState.Chase:
                CheckToRange(attackRange, MonsterState.Attack, chaseRange, MonsterState.Wander); // ���� ���� ���̸� ���� ����, ���� ���� ���̸� ��ȸ ����
                Chase();
                break;

            case MonsterState.Attack:
                FallBackState(attackRange, MonsterState.Chase); // ���� ���� ���̸� ���� ����
                if (attackCooldownTimer <= 0) // ���� ��Ÿ��
                {
                    StartCoroutine(LookAtPlayer());
                    StartCoroutine(Attack());
                    attackCooldownTimer = attackCooldown;
                }
                break;

            case MonsterState.Death:
                if (!isDead)
                {
                    Death();
                    isDead = true;
                }
                break;
        }
    }

    protected virtual void Wander()
    {
        if (agent.remainingDistance <= agent.stoppingDistance && !agent.pathPending) // ��ǥ�� �������� ���� ���� ��ġ ã��
        {
            Vector3 randomDir = Random.insideUnitSphere * moveRadius; // �� ��ġ�� �������� ������ ��ġ ã��
            randomDir += transform.position;

            randomDir.y = transform.position.y; // �������θ� �̵��ϵ��� ����

            if (Vector3.Distance(transform.position, randomDir) < 2f)  // ��ǥ ��ġ�� �ʹ� ������ �ٽ� ���
            {
                return;
            }

            if (Physics.CheckSphere(randomDir, 0.5f, obstaclesLayer)) // ��ֹ� ���̾� üũ �� ������ �ش� ��ġ�� �̵�
            {
                return; // �浹 ������ �̵� X
            }

            agent.SetDestination(randomDir); // �� ������ ����
        }

        MoveBlendTree();
    }

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
        if (agent != null)
        {
            agent.isStopped = true;
            agent.enabled = false;
        }
        animator.SetTrigger("Death");
        StartCoroutine(DestroyObject(3.0f));
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