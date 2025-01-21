using System.Collections;
using UnityEngine;

public class MonsterSkeleton : MonsterBase
{
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
