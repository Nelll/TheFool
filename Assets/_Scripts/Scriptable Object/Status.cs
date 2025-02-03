using UnityEngine;

[CreateAssetMenu(fileName = "Status", menuName = "Scriptable Objects/Status")]
public class Status : ScriptableObject
{
    [SerializeField] private GameObject prefab;
    [SerializeField] private string name;
    [SerializeField] private int health;
    [SerializeField] private int mentalPoint;
    [SerializeField] private int attack;
    [SerializeField] private int ultimateAttackDamage; // �ñر� ������
    [SerializeField] private int defense;

    public GameObject Prefab
    { get { return prefab; } }
    public string Name
    { get { return name; } }
    public int Health
    { get { return health; } }
    public int MentalPoint
    { get { return mentalPoint; } }
    public int Attack
    { get { return attack; } }
    public int UltimateAttackDamage
    { get { return ultimateAttackDamage; } }
    public int Defense
    { get { return defense; } }
}
