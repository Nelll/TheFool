using System.Runtime.InteropServices;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5.0f;    // �ӵ�
    [SerializeField] private float runSpeed = 8.0f;     // �޸��� �ӵ�
    [SerializeField] private float jumpPower = 3.0f;    // ����
    private float gravity = -9.81f; // �߷�
    private Vector3 moveDirection;  // �̵�����

    private CharacterController characterController;
    private bool isRunning = false;

    private void Awake()
    {
        characterController = GetComponent<CharacterController>();
    }

    private void Update()
    {
        if (!characterController.isGrounded)
        {
            
            moveDirection.y += gravity * Time.deltaTime;
        }
        float currentSpeed = isRunning ? runSpeed : moveSpeed;      // �޸��� ���¿� ���� �ӵ� ����
        characterController.Move(moveDirection * moveSpeed * Time.deltaTime);
    }

    // �ܺο��� ȣ���ؼ� �Ű������� ���������� ��������, ���� ������ moveDirection�� ����
    public void Move(Vector3 direction)
    {
        moveDirection = new Vector3(direction.x, moveDirection.y, direction.z);
    }

    public void Jump()
    {
        
        if (characterController.isGrounded)
        {
            moveDirection.y = jumpPower;
        }
    }

    public void Run(bool isRun)
    {
        isRunning = isRun;
    }

    
}
