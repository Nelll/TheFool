using UnityEngine;

public abstract class MonsterBase : MonoBehaviour
{
    protected Transform target;
    protected bool isDead = false;

    void Awake() => target = GameObject.FindWithTag("Player").GetComponent<Transform>();

    void Update()
    {
        if (isDead) return;
    }
        
    protected virtual void Wander() => Debug.Log("순회 및 탐색 코드 작성");
    protected virtual void Chase() => Debug.Log("추적 코드 작성");
    protected virtual void Attack() => Debug.Log("공격 코드 작성");
    protected virtual void Dead()
    {
        isDead = true;
        Destroy(gameObject);
    }

}
