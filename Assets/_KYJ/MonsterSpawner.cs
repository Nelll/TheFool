using UnityEngine;

public enum MonsterType { Skeleton, Bug };

public class MonsterSpawner : MonoBehaviour
{
    public MonsterType monsterType;
    public Transform[] spawnPoints;
    public GameObject[] monsterPrefabs;
    [SerializeField] Transform parentObject;

    void Start()
    {
        SpawnMonster();
    }

    void SpawnMonster()
    {
        GameObject monsterPrefab = null;

        switch (monsterType)
        {
            case MonsterType.Skeleton:
                monsterPrefab = monsterPrefabs[0];
                break;
            case MonsterType.Bug:
                monsterPrefab = monsterPrefabs[1];
                break;
        }

        if (monsterPrefab != null && spawnPoints.Length > 0)
        {
            foreach (Transform spawnPosition in spawnPoints)
            {
                Instantiate(monsterPrefab, spawnPosition.position, Quaternion.identity, parentObject);
            }
        }
    }
}