using System;
using UnityEngine;

public class Health : MonoBehaviour
{

    public int maxHealth;
    public int currentHealth;

    public bool isDead;

    public Action<Health> OnDie;

    Status status;
    
    private void Awake()
    {
        status = GetComponent<Status>();
        maxHealth = status.Health;
        currentHealth = maxHealth;
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

            OnDie?.Invoke(this);
        }
    }
}
