using UnityEngine;
using System.Collections;

public class MonsterSkeleton : MonsterBase
{
    /// <summary>
    /// ���� ������ �̵� �ִϸ��̼� ����� �۵��ϰԲ� �Ƚ�
    /// �����Ҷ� �÷��̾� �ٶ󺸰� ����
    /// </summary>

    int curretWaypointIndex = 0;

    void Awake()
    {
        chaseRange = 7.0f;
        attackRange = 2.0f;
    }

    protected override void Update()
    {
        base.Update();
        Debug.Log(animator.GetCurrentAnimatorStateInfo(0).length);
    }

    protected override void Wander()
    {
        if (waypoints.Length == 0)
        {
            Debug.LogError($"{transform.name}�� Wander �Լ� ���� �߻�. \n��������Ʈ�� �����ϴ�.");
            return;
        }

        MoveToWaypoint(); // ��������Ʈ �̵�
        MoveBlendTree(); // �̵� ���� Ʈ��

        if (Vector3.Distance(transform.position, waypoints[curretWaypointIndex].position) <= 1.1f) // ��������Ʈ ��ȸ
        {
            curretWaypointIndex++;
            
            if (curretWaypointIndex == waypoints.Length) curretWaypointIndex = 0; // ������ ��������Ʈ ���� �� ó������
        }
    }

    protected override void Chase()
    {
        if (target == null)
        {
            Debug.LogError($"{transform.name}�� Chase �Լ� ���� �߻�.\nŸ�� �÷��̾ �������� �ʾҽ��ϴ�.");
            return;
        }

        MoveToTarget(); // ��������Ʈ �̵�
        MoveBlendTree(); // �̵� ���� Ʈ��
    }

    protected override IEnumerator Attack()
    {
        if (isAttacking) yield break; // ���� ���϶� �ݺ����� ����

        isAttacking = true; // ���� ����
        agent.isStopped = true; // ���� ���̴� AI �߰� ����
        int randomIndex = Random.Range(0, 2);

        if (randomIndex == 0)
        {
            animator.SetTrigger("Attack");
            yield return new WaitForSeconds(2.633f);
            isAttacking = false; // ���� ��
            agent.isStopped = false; // ���� �������� AI �߰� Ȱ��ȭ
        }
        else
        {
            animator.SetTrigger("Punch");
            yield return new WaitForSeconds(3.833f);
            isAttacking = false; // ���� ��    
            agent.isStopped = false; // ���� �������� AI �߰� Ȱ��ȭ
        }
    }

    protected new void Death()
    {
        agent.isStopped = true;
        animator.SetTrigger("Death");
        base.Death();
    }

    void MoveToWaypoint() // ��������Ʈ�� �̵��ϴ� �Լ�
    {
        if (waypoints.Length == 0)
        {
            Debug.LogError($"{transform.name}�� MoveToWaypoint �Լ� ���� �߻�.\n��������Ʈ �迭�� ����ֽ��ϴ�.");
            return;
        }

        agent.SetDestination(waypoints[curretWaypointIndex].position); // AI �������� ��������Ʈ�� ����
    }

    void MoveToTarget() // �÷��̾ �����ϴ� �Լ�
    {
        if (target == null)
        {
            Debug.LogError($"{transform.name}�� MoveToTarget �Լ� ���� �߻�.\nŸ�� �÷��̾ �������� �ʾҽ��ϴ�.");
            return;
        }
        
        agent.SetDestination(target.position); // AI �������� �÷��̾�� ����
    }

    void MoveBlendTree() // �̵� ���� Ʈ��
    {
        float targetMagnitude = agent.velocity.magnitude > 0.1f ? 1f : 0f; // �����ӿ� ���� �ִϸ��̼� ��ȯ
        float currentMagnitude = animator.GetFloat("SpeedMagnitude"); // ���� �Ķ����
        float smoothMagnitude = Mathf.Lerp(currentMagnitude, targetMagnitude, Time.deltaTime * 1.5f); // �����ϰ� �����
        animator.SetFloat("SpeedMagnitude", smoothMagnitude); // �Ҵ�
    }
}
