using UnityEngine;

public class StageManager : MonoBehaviour
{
    [SerializeField] float detectedRange = 10f;
    [SerializeField] GameObject invisibleWall;

    private void Awake()
    {
        invisibleWall.SetActive(true);
    }

    private void Update()
    {
        CheckDetectedMonster();
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectedRange);
    }

    void CheckDetectedMonster()
    {
        Collider[] overlaps = Physics.OverlapSphere(transform.position, detectedRange, LayerMask.GetMask("Monster"));

        if(overlaps.Length <= 0)
        {
            invisibleWall.SetActive(false);
        }
    }
}
