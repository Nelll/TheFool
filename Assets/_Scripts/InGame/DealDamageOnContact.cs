using JetBrains.Annotations;
using UnityEngine;

public class DealDamageOnContact : MonoBehaviour
{
    [SerializeField] GameObject prefab;

    Health health;
    int damage;


    private void OnTriggerEnter(Collider other)
    {
        // 플레이어가 데미지를 받을 경우
        if(other.CompareTag("Player") && other.GetComponent<Health>().isInvincibilityTime == false)
        {
            if (prefab.CompareTag("BossMonster"))
            { // 데미지를 준게 보스 몬스터인 경우
                damage = prefab.GetComponent<BossBehaviorAI>().Damage;
            }
            if(prefab.CompareTag("Monster"))
            { // 데미지를 준게 일반 몬스터인 경우
                damage = prefab.GetComponent<DamageManager>().Damage;
            }
            other.GetComponent<Health>().TakeDamage(damage);
            Debug.Log("Player가 " + damage + " 데미지를 입었습니다.");
            other.GetComponent<Health>().isInvincibilityTime = true;
        }

        // 보스 몬스터가 데미지를 받을 경우
        if(other.CompareTag("BossMonster") && other.GetComponentInParent<Health>().isInvincibilityTime == false)
        {
            damage = prefab.GetComponent<DamageManager>().Damage;
            other.GetComponentInParent<Health>().TakeDamage(damage);
            Debug.Log("BossMonster가 " + damage + " 데미지를 입었습니다.");
            other.GetComponentInParent<Health>().isInvincibilityTime = true;
        }

        // 일반 몬스터가 데미지를 받을 경우
        if (other.CompareTag("Monster") && other.GetComponent<Health>().isInvincibilityTime == false)
        {
            damage = prefab.GetComponent<DamageManager>().Damage;
            other.GetComponent<Health>().TakeDamage(damage);
            Debug.Log("Monster가 " + damage + " 데미지를 입었습니다.");
            other.GetComponent<Health>().isInvincibilityTime = true;
        }

    }
}
