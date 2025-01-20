using System.Collections;
using UnityEngine;

public class MonsterBug : MonsterBase
{
    public float moveRadius; // 랜덤 이동할 원의 지름
    public LayerMask obstaclesLayer; // 장애물로 인지할 레이어

    void Awake()
    {
        chaseRange = 2.5f;
        attackRange = 2.0f;
        moveRadius = 3.0f;
    }

    protected override void Wander()
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
