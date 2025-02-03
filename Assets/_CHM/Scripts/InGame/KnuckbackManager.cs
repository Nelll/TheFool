using UnityEngine;

public class KnuckbackManager : MonoBehaviour
{
    [SerializeField] float knockbackDuration = 0.5f;

    public bool isKnockback = false;   // �˹� ���� ����
    float knockbackTimer = 0f;  // �˹� ���� �ð� Ÿ�̸�
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
