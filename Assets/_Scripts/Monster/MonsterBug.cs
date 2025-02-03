using System.Collections;
using UnityEngine;

public class MonsterBug : MonsterBase
{
    protected override IEnumerator Attack()
    {
        if (isAttacking || agent == null || !agent.isOnNavMesh) yield break;

        isAttacking = true;

        agent.isStopped = true;

        float animationDuration = 0f;

        if (distanceToPlayer >= 1.5f)
        {
            animator.SetTrigger("Stinger");
            animationDuration = 0.667f;
        }
        else
        {
            int randomIndex = Random.Range(0, 2);

            switch (randomIndex)
            {
                case 0:
                    animator.SetTrigger("Roar");
                    animationDuration = 2.333f;
                    break;
                case 1:
                    animator.SetTrigger("Dash");
                    animationDuration = 0.667f;
                    break;
            }
        }
        yield return new WaitForSeconds(animationDuration);

        isAttacking = false;

        if (agent != null && agent.isOnNavMesh)
        {
            agent.isStopped = false;
        }

        animator.SetTrigger("Finish");
    }
}
