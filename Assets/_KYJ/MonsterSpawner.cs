using UnityEngine;

public enum MonsterType { Skeleton, Bug };

public class MonsterSpawner : MonoBehaviour
{
    public MonsterType monsterType;
    public Transform[] spawnPositions;
    public GameObject[] monsterPrefabs;

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

        if (monsterPrefab != null && spawnPositions.Length > 0)
        {
            foreach (Transform spawnPosition in spawnPositions)
            {
                Instantiate(monsterPrefab, spawnPosition.position, Quaternion.identity);
            }
        }
        else
        {
            Debug.LogError("몬스터 프리팹 또는 스폰 위치가 제대로 설정되지 않았습니다.");
        }
    }
}