using JetBrains.Annotations;
using UnityEngine;

public class DealDamageOnContact : MonoBehaviour
{
    [SerializeField] GameObject prefab;

    Health health;
    int damage;


    private void OnTriggerEnter(Collider other)
    {

        if(other.CompareTag("Player") && other.GetComponent<Health>().isInvincibilityTime == false)
        {
            damage = prefab.GetComponent<BossBehaviorAI>().Damage;
            other.GetComponent<Health>().TakeDamage(damage);
            Debug.Log("Player�� " + damage + " �������� �Ծ����ϴ�.");
            other.GetComponent<Health>().isInvincibilityTime = true;
        }

        if(other.CompareTag("MonsterDamage") && other.GetComponentInParent<Health>().isInvincibilityTime == false)
        {
            damage = prefab.GetComponent<DamageManagerTest>().Damage;
            other.GetComponentInParent<Health>().TakeDamage(damage);
            Debug.Log("BossMonster�� " + damage + " �������� �Ծ����ϴ�.");
            other.GetComponentInParent<Health>().isInvincibilityTime = true;
        }

    }
}
