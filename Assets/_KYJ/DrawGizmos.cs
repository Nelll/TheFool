using UnityEngine;

public class DrawGizmos : MonoBehaviour
{
    public float redRange;
    public float orangeRange;
    public float yellowRange;
    public float greenRange;
    public float blueRange;
    public float whiteRange;

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, redRange);

        Gizmos.color = new Color(1f, 0.4f, 0f);
        Gizmos.DrawWireSphere(transform.position, orangeRange);

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, yellowRange);

        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, greenRange);

        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, blueRange);

        Gizmos.color = Color.white;
        Gizmos.DrawWireSphere(transform.position, whiteRange);
    }
}
