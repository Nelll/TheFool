using UnityEngine;

public class DrawGizmos : MonoBehaviour
{
    [Tooltip("공격 범위")] float attackRange;
    [Tooltip("추적 범위")] float chaseRange;
    [Tooltip("이동 반경")] float moveRadius;

    void Start()
    {
        attackRange = GetComponent<MonsterBase>().AttackRange;
        chaseRange = GetComponent<MonsterBase>().ChaseRange;
        moveRadius = GetComponent<MonsterBase>().MoveRadius;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);

        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, chaseRange);

        Gizmos.color = Color.white;
        Gizmos.DrawWireSphere(transform.position, moveRadius);
    }
}
