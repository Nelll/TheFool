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
            Debug.LogError("MonsterBase.cs에서 Target 지정이 실패했습니다. 스크립트가 비활성화 됩니다");
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
            Debug.LogError($"{transform.name}의 Chase 함수 에러 발생.\n타겟 플레이어가 감지되지 않았습니다.");
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
                /// 추적 범위 안이면 추적 상태로
                /// </summary>
                CheckToRange(chaseRange, MonsterState.Chase);
                Wander();
                break;

            case MonsterState.Chase:
                /// <summary> 
                /// 공격 범위 안이면 공격 상태로,
                /// 추적 범위 밖이면 배회 상태로
                /// </summary>
                CheckToRange(attackRange, MonsterState.Attack, chaseRange, MonsterState.Wander);
                Chase();
                break;

            case MonsterState.Attack:
                /// <summary>
                /// 공격 범위 밖이면 추적 상태로
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

    protected void MoveToWaypoint() // 웨이포인트로 이동
    {
        if (waypoints.Length == 0)
        {
            Debug.LogError($"{transform.name}의 MoveToWaypoint 함수 에러 발생.\n웨이포인트 배열이 비어있습니다.");
            return;
        }

        agent.SetDestination(waypoints[curretWaypointIndex].position);
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