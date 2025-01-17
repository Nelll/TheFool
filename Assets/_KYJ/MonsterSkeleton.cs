using UnityEngine;
using System.Collections;

public class MonsterSkeleton : MonsterBase
{
    /// <summary>
    /// 공격 끝나고 이동 애니메이션 제대로 작동하게끔 픽스
    /// 공격할때 플레이어 바라보게 설정
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
            Debug.LogError($"{transform.name}의 Wander 함수 에러 발생. \n웨이포인트가 없습니다.");
            return;
        }

        MoveToWaypoint(); // 웨이포인트 이동
        MoveBlendTree(); // 이동 블렌드 트리

        if (Vector3.Distance(transform.position, waypoints[curretWaypointIndex].position) <= 1.1f) // 웨이포인트 배회
        {
            curretWaypointIndex++;
            
            if (curretWaypointIndex == waypoints.Length) curretWaypointIndex = 0; // 마지막 웨이포인트 도착 시 처음으로
        }
    }

    protected override void Chase()
    {
        if (target == null)
        {
            Debug.LogError($"{transform.name}의 Chase 함수 에러 발생.\n타겟 플레이어가 감지되지 않았습니다.");
            return;
        }

        MoveToTarget(); // 웨이포인트 이동
        MoveBlendTree(); // 이동 블렌드 트리
    }

    protected override IEnumerator Attack()
    {
        if (isAttacking) yield break; // 공격 중일때 반복실행 방지

        isAttacking = true; // 공격 시작
        agent.isStopped = true; // 공격 중이니 AI 추격 중지
        int randomIndex = Random.Range(0, 2);

        if (randomIndex == 0)
        {
            animator.SetTrigger("Attack");
            yield return new WaitForSeconds(2.633f);
            isAttacking = false; // 공격 끝
            agent.isStopped = false; // 공격 끝났으니 AI 추격 활성화
        }
        else
        {
            animator.SetTrigger("Punch");
            yield return new WaitForSeconds(3.833f);
            isAttacking = false; // 공격 끝    
            agent.isStopped = false; // 공격 끝났으니 AI 추격 활성화
        }
    }

    protected new void Death()
    {
        agent.isStopped = true;
        animator.SetTrigger("Death");
        base.Death();
    }

    void MoveToWaypoint() // 웨이포인트로 이동하는 함수
    {
        if (waypoints.Length == 0)
        {
            Debug.LogError($"{transform.name}의 MoveToWaypoint 함수 에러 발생.\n웨이포인트 배열이 비어있습니다.");
            return;
        }

        agent.SetDestination(waypoints[curretWaypointIndex].position); // AI 목적지를 웨이포인트로 설정
    }

    void MoveToTarget() // 플레이어를 추적하는 함수
    {
        if (target == null)
        {
            Debug.LogError($"{transform.name}의 MoveToTarget 함수 에러 발생.\n타겟 플레이어가 감지되지 않았습니다.");
            return;
        }
        
        agent.SetDestination(target.position); // AI 목적지를 플레이어로 설정
    }

    void MoveBlendTree() // 이동 블렌드 트리
    {
        float targetMagnitude = agent.velocity.magnitude > 0.1f ? 1f : 0f; // 움직임에 따라 애니메이션 전환
        float currentMagnitude = animator.GetFloat("SpeedMagnitude"); // 현재 파라미터
        float smoothMagnitude = Mathf.Lerp(currentMagnitude, targetMagnitude, Time.deltaTime * 1.5f); // 러프하게 만들기
        animator.SetFloat("SpeedMagnitude", smoothMagnitude); // 할당
    }
}
