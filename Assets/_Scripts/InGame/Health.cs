using InfinityPBR;
using System;
using UnityEngine;

public class Health : MonoBehaviour
{
    public float invincibilityTime = 2f;
    public int maxHealth;
    public int currentHealth;

    public bool isDead;
    public bool isInvincibilityTime = false;
    float count = 0;

    public Status status;
    
    private void Awake()
    {
        maxHealth = status.Health;
        currentHealth = maxHealth;
    }

    private void Update()
    {
        if (isInvincibilityTime == true)
        {
            count += Time.deltaTime;
            if(count >= invincibilityTime)
            {
                isInvincibilityTime = false;
                count = 0;
            }
        }
    }

    public void TakeDamage(int damage)
    {
        ModifyHealth(-damage);
    }

    void ModifyHealth(int value)
    {
        if (isDead) { return; }
        int newHealth = currentHealth + value;
        currentHealth = Mathf.Clamp(newHealth, 0, maxHealth);

        if(currentHealth == 0)
        {
            isDead = true;
            OnDie();
            //Invoke("OnDie", 3f);
        }
    }

    void OnDie()
    {
        if(this.CompareTag("Player"))
        {
            GameManager.Instance.LoadGameOver();
        }
        if(this.CompareTag("BossMonster"))
        {
            GameManager.Instance.LoadGameWin();
        }
        Destroy(this.gameObject);
    }
}
