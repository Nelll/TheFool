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
    [SerializeField] LayerMask obstaclesLayer; // 장애물로 인지할 레이어
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
            Debug.LogError("MonsterBase.cs에서 Target 지정이 실패했습니다. 스크립트가 비활성화 됩니다");
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
                CheckToRange(chaseRange, MonsterState.Chase); // 추적 범위 안이면 추적 상태
                Wander();
                break;

            case MonsterState.Chase:
                CheckToRange(attackRange, MonsterState.Attack, chaseRange, MonsterState.Wander); // 공격 범위 안이면 공격 상태, 추적 범위 밖이면 배회 상태
                Chase();
                break;

            case MonsterState.Attack:
                FallBackState(attackRange, MonsterState.Chase); // 공격 범위 밖이면 추적 상태
                if (attackCooldownTimer <= 0) // 공격 쿨타임
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
        if (agent.remainingDistance <= agent.stoppingDistance && !agent.pathPending) // 목표에 도달했을 때만 다음 위치 찾기
        {
            Vector3 randomDir = Random.insideUnitSphere * moveRadius; // 현 위치를 기준으로 랜덤한 위치 찾기
            randomDir += transform.position;

            randomDir.y = transform.position.y; // 수평으로만 이동하도록 고정

            if (Vector3.Distance(transform.position, randomDir) < 2f)  // 목표 위치가 너무 가까우면 다시 계산
            {
                return;
            }

            if (Physics.CheckSphere(randomDir, 0.5f, obstaclesLayer)) // 장애물 레이어 체크 후 없으면 해당 위치로 이동
            {
                return; // 충돌 있으면 이동 X
            }

            agent.SetDestination(randomDir); // 새 목적지 설정
        }

        MoveBlendTree();
    }

    protected virtual void Chase()
    {
        if (target == null)
        {
            Debug.LogError($"{transform.name}의 Chase 함수 에러 발생.\n타겟 플레이어가 감지되지 않았습니다.");
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
    IEnumerator DestroyObject(float destroyTime) // Death 호출시 오브젝트 파괴
    {
        yield return new WaitForSeconds(destroyTime);
        Destroy(gameObject);
    }

    void DistanceToPlayer() // 플레이어 와의 거리
    {
        if (target != null)
        {
            distanceToPlayer = Vector3.Distance(transform.position, target.position);
        }
    }

    void CheckToRange(float rangeType, MonsterState wantState) // 거리에 따른 상태 변화
    {
        if (distanceToPlayer <= rangeType)
        {
            monsterState = wantState;
        }
    }

    void CheckToRange(float rangeType, MonsterState wantState, float fallBackRangeType, MonsterState fallBackState) // 거리에 따른 상태 변화
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

    void FallBackState(float rangeType, MonsterState wantState) // 이전 상태로 돌아가게
    {
        if (distanceToPlayer > rangeType)
        {
            monsterState = wantState;
        }
    }

    protected IEnumerator LookAtPlayer() // 플레이어 바라보기
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

    protected void MoveToTarget() // 플레이어 추적
    {
        if (target == null)
        {
            Debug.LogError($"{transform.name}의 MoveToTarget 함수 에러 발생.\n타겟 플레이어가 감지되지 않았습니다.");
            return;
        }

        agent.SetDestination(target.position);
    }

    protected void MoveBlendTree() // 이동 블렌드트리 변경
    {
        float targetMagnitude = agent.velocity.magnitude > 0.1f ? 1f : 0f;
        float currentMagnitude = animator.GetFloat("SpeedMagnitude"); 
        float smoothMagnitude = Mathf.Lerp(currentMagnitude, targetMagnitude, Time.deltaTime * 1.5f);
        animator.SetFloat("SpeedMagnitude", smoothMagnitude);
    }

    void SetCooldown() // 공격 쿨타임
    {
        if (attackCooldown > 0)
        {
            attackCooldownTimer -= Time.deltaTime;
        }
    }
    #endregion
}