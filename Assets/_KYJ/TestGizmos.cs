using UnityEngine;

public class TestGizmos : MonoBehaviour
{
    public float range;

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, range);
    }
}
