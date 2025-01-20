using System.Collections;
using UnityEngine;

public class MonsterSkeleton : MonsterBase
{
    void Awake()
    {
        chaseRange = 5.0f;
        attackRange = 2.0f;
    }
    
    protected override void Wander()
    {
        if (waypoints.Length == 0)
        {
            Debug.LogError($"{transform.name}의 Wander 함수 에러 발생. \n웨이포인트가 없습니다.");
            return;
        }

        MoveToWaypoint();
        MoveBlendTree();

        if (Vector3.Distance(transform.position, waypoints[curretWaypointIndex].position) <= 1.1f) // 웨이포인트 배회
        {
            curretWaypointIndex++;

            if (curretWaypointIndex == waypoints.Length) curretWaypointIndex = 0; // 마지막 웨이포인트 도착 시 처음으로
        }
    }

    protected override IEnumerator Attack()
    {
        if (isAttacking) yield break;
        isAttacking = true;
        agent.isStopped = true;

        int randomIndex = Random.Range(0, 2);
        string trigger = randomIndex == 0 ? "Attack" : "Punch";
        float animationDuration = randomIndex == 0 ? 2.633f : 3.833f;

        animator.SetTrigger(trigger);
        yield return new WaitForSeconds(animationDuration);

        isAttacking = false;
        agent.isStopped = false;
        animator.SetTrigger("Finish");
    }
}
