using UnityEngine;

public class DrawGizmos : MonoBehaviour
{
    float attackRange; // 공격 범위
    float chaseRange; // 추적 범위
    float moveRadius; // 이동 반경

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
