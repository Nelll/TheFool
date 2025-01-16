using System.Runtime.InteropServices;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5.0f;    // 속도
    [SerializeField] private float runSpeed = 8.0f;     // 달리기 속도
    [SerializeField] private float jumpPower = 3.0f;    // 점프
    private float gravity = -9.81f;                     // 중력
    private Vector3 moveDirection;                      // 이동방향

    private CharacterController characterController;
    private bool isRunning = false;                     // 달리기 상태 변수

    private void Awake()
    {
        characterController = GetComponent<CharacterController>();
    }

    private void Update()
    {
        // 중력 설정, 캐릭터가 땅을 밟지 않은 상태면 y축 방향에 gravity * Time.deltaTime을 더해 중력 적용
        if (!characterController.isGrounded)
        {

            moveDirection.y += gravity * Time.deltaTime;
        }
        // 현재 이동 속도: 달리기 상태라면 runSpeed, 그렇지 않으면 moveSpeed 사용 (달리기 상태에 따라 속도 변경)
        float currentSpeed = isRunning ? runSpeed : moveSpeed;

        // 이동 처리: 방향과 속도를 기반으로 캐릭터를 이동
        characterController.Move(moveDirection * moveSpeed * Time.deltaTime);
    }

    // 외부에서 호출해서 매개변수의 방향정보를 가져오면, 방향 정보를 moveDirection에 저장
    public void Move(Vector3 direction)
    {
        // 입력받은 방향을 moveDirection에 저장
        moveDirection = new Vector3(direction.x, moveDirection.y, direction.z);
    }

    public void Jump()
    {
        // 캐릭터가 바닥을 밟고 있으면, 점프
        if (characterController.isGrounded)
        {
            moveDirection.y = jumpPower;
        }
    }

    // 달리기 상태를 설정
    public void Run(bool isRun)
    {
        isRunning = isRun;
    }
}
