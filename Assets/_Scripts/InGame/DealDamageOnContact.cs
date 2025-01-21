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
            if (prefab.CompareTag("BossMonster"))
            {
                damage = prefab.GetComponent<BossBehaviorAI>().Damage;
            }
            if(prefab.CompareTag("Monster"))
            {
                damage = prefab.GetComponent<DamageManager>().Damage;
            }
            other.GetComponent<Health>().TakeDamage(damage);
            Debug.Log("Player�� " + damage + " �������� �Ծ����ϴ�.");
            other.GetComponent<Health>().isInvincibilityTime = true;
        }

        if(other.CompareTag("BossMonster") && other.GetComponentInParent<Health>().isInvincibilityTime == false)
        {
            damage = prefab.GetComponent<DamageManager>().Damage;
            other.GetComponentInParent<Health>().TakeDamage(damage);
            Debug.Log("BossMonster�� " + damage + " �������� �Ծ����ϴ�.");
            other.GetComponentInParent<Health>().isInvincibilityTime = true;
        }

        if (other.CompareTag("Monster") && other.GetComponent<Health>().isInvincibilityTime == false)
        {
            damage = prefab.GetComponent<DamageManager>().Damage;
            other.GetComponent<Health>().TakeDamage(damage);
            Debug.Log("Monster�� " + damage + " �������� �Ծ����ϴ�.");
            other.GetComponent<Health>().isInvincibilityTime = true;
        }

    }
}
