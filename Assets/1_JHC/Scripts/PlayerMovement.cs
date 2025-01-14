using System.Runtime.InteropServices;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5.0f;    // 속도
    [SerializeField] private float runSpeed = 8.0f;     // 달리기 속도
    [SerializeField] private float jumpPower = 3.0f;    // 점프
    private float gravity = -9.81f; // 중력
    private Vector3 moveDirection;  // 이동방향

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
        float currentSpeed = isRunning ? runSpeed : moveSpeed;      // 달리기 상태에 따라 속도 변경
        characterController.Move(moveDirection * moveSpeed * Time.deltaTime);
    }

    // 외부에서 호출해서 매개변수의 방향정보를 가져오면, 방향 정보를 moveDirection에 저장
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
