using UnityEngine;

public class KnuckbackManager : MonoBehaviour
{
    [SerializeField] float knockbackDuration = 0.5f;

    public bool isKnockback = false;   // 넉백 상태 여부
    float knockbackTimer = 0f;  // 넉백 지속 시간 타이머
    Rigidbody rb;



    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        if (isKnockback == true)
        {
            knockbackTimer += Time.deltaTime;
        }
        if (knockbackTimer > knockbackDuration)
        {
            rb.linearVelocity = Vector3.zero;
            isKnockback = false;
            knockbackTimer = 0;
        }
    }
}
