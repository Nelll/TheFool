using UnityEngine;

public class DrawGizmos : MonoBehaviour
{
    float attackRange; // ���� ����
    float chaseRange; // ���� ����
    float moveRadius; // �̵� �ݰ�

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
