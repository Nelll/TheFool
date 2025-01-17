using UnityEngine;
using UnityEngine.AI;
using System.Collections;

public enum MonsterState { Wander, Chase, Attack, Death }

public abstract class MonsterBase : MonoBehaviour
{
    public MonsterState monsterState;
    [SerializeField] protected Transform target;
    [SerializeField] float distanceToPlayer; // 플레이어와의 거리
    [SerializeField] protected float chaseRange; // 추적 행동을 실행할 범위
    [SerializeField] protected float attackRange; // 공격 행동을 실행할 범위
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
            Debug.LogError("MonsterBase.cs에서 Target 지정이 실패했습니다. 스크립트가 비활성화 됩니다");
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
        StartCoroutine(DestroyObject(5.0f)); // 5초 뒤 오브젝트 제거
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
