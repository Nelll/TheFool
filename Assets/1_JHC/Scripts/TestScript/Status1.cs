using UnityEngine;

[CreateAssetMenu(fileName = "Status1", menuName = "Scriptable Objects/Status1")]
public class Status1 : ScriptableObject
{
    [SerializeField] private GameObject prefab;
    [SerializeField] private string name;
    [SerializeField] private int health;
    [SerializeField] private float mentalPoint;
    [SerializeField] private int attack;
    [SerializeField] private int ultimateAttackDamage; // ±Ã±Ø±â µ¥¹ÌÁö
    [SerializeField] private int defense;

    public GameObject Prefab
    { get { return prefab; } }
    public string Name
    { get { return name; } }
    public int Health
    { get { return health; } }
    public float MentalPoint
    { get { return mentalPoint; } }
    public int Attack
    { get { return attack; } }
    public int UltimateAttackDamage
    { get { return ultimateAttackDamage; } }
    public int Defense
    { get { return defense; } }
}
