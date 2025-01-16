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
        
    protected virtual void Wander() => Debug.Log("��ȸ �� Ž�� �ڵ� �ۼ�");
    protected virtual void Chase() => Debug.Log("���� �ڵ� �ۼ�");
    protected virtual void Attack() => Debug.Log("���� �ڵ� �ۼ�");
    protected virtual void Dead()
    {
        isDead = true;
        Destroy(gameObject);
    }

}
