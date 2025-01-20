using Mono.Cecil;
using UnityEngine;

public class DamageManagerTest : MonoBehaviour
{
    public Status status;

    int damage;

    public int Damage
    {
        get { return damage; }
        set { damage = value; }
    }

    private void Start()
    {
        damage = status.Attack;
    }
}
