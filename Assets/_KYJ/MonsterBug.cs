using System.Collections;
using UnityEngine;

public class MonsterBug : MonsterBase
{
    public float moveRadius; // ���� �̵��� ���� ����
    public LayerMask obstaclesLayer; // ��ֹ��� ������ ���̾�

    void Awake()
    {
        chaseRange = 2.5f;
        attackRange = 2.0f;
        moveRadius = 3.0f;
    }

    protected override void Wander()
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

    protected override IEnumerator Attack()
    {
        if (isAttacking) yield break;
        isAttacking = true;
        agent.isStopped = true;

        int randomIndex = Random.Range(0, 3);
        float animationDuration = 0f;

        switch (randomIndex)
        {
            case 0:
                animator.SetTrigger("Roar1");
                animationDuration = 2.333f;
                break;
            case 1:
                animator.SetTrigger("Roar2");
                animationDuration = 1.333f;
                break;
            case 2:
                animator.SetTrigger("Attack");
                animationDuration = 0.667f;
                break;
        }
        yield return new WaitForSeconds(animationDuration);

        isAttacking = false;
        agent.isStopped = false;
        animator.SetTrigger("Finish");
    }
}
