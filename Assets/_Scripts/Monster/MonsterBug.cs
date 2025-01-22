using System.Collections;
using UnityEngine;

public class MonsterBug : MonsterBase
{
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
