using UnityEngine;

public class BossControllerTest : MonoBehaviour
{
    [SerializeField]
    private GameObject bossPrefab;
    [SerializeField]
    private Transform target;

    [System.Serializable]
    private struct WayPointData
    {
        public GameObject[] wayPoints;
    }
    [SerializeField]
    private WayPointData[] wayPointData;

    private void Awake()
    {
        int wayIndex = Random.Range(0, wayPointData.Length-1);

        bossPrefab.GetComponent<EnemyFSMTest>().Setup(target, wayPointData[wayIndex].wayPoints);
    }
}
