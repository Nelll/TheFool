using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5.0f;    // 속도
    [SerializeField] private float runSpeed = 8.0f;     // 달리기 속도

    [SerializeField] private float rollSpeed = 10.0f;   // 구르기 속도
    [SerializeField] private float rollDuration = 1.05f; // 구르기 시간

    [SerializeField] private float jumpPower = 3.0f;    // 점프
    private float gravity = -9.81f;                     // 중력
    private Vector3 moveDirection;                      // 이동방향

    private CharacterController characterController;
    private bool isRolling = false; // 구르기 상태 변수
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

        if (!isRolling)
        {
            // 현재 이동 속도: 달리기 상태라면 runSpeed, 그렇지 않으면 moveSpeed 사용 (달리기 상태에 따라 속도 변경)
            float currentSpeed = isRunning ? runSpeed : moveSpeed;

            // 이동 처리: 방향과 속도를 기반으로 캐릭터를 이동
            characterController.Move(moveDirection * moveSpeed * Time.deltaTime);
        }
    }

    // 외부에서 호출해서 매개변수의 방향정보를 가져오면, 방향 정보를 moveDirection에 저장
    public void Move(Vector3 direction)
    {
        if (!isRolling)
        {
            // 입력받은 방향을 moveDirection에 저장
            moveDirection = new Vector3(direction.x, moveDirection.y, direction.z);
        }
    }

    public bool IsGrounded()
    {
        return characterController.isGrounded;
    }

    //public void Jump()
    //{
    //    if (characterController.isGrounded)
    //    {
    //        moveDirection.y = jumpPower;
    //        StartCoroutine(DisableGravityForMoment());
    //    }
    //}

    private IEnumerator DisableGravityForMoment()
    {
        float gravityBackup = gravity;
        gravity = 0; // 중력 비활성화
        yield return new WaitForSeconds(0.2f); // 일정 시간 대기
        gravity = gravityBackup; // 중력 복구
    }

    // 달리기 상태를 설정
    public void Run(bool isRun)
    {
        isRunning = isRun;
    }

    public void Roll()
    {
        if (!isRolling)
        {
            StartCoroutine(RollCoroutine());
        }
    }

    private IEnumerator RollCoroutine()
    {
        isRolling = true; // 구르기 시작
        Vector3 rollDirection = transform.forward; // 구르기 방향
        moveDirection = Vector3.zero;

        float elapsedTime = 0f;
        while (elapsedTime < rollDuration)
        {
            characterController.Move(rollDirection * rollSpeed * Time.deltaTime); // 구르기 이동
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        moveDirection = Vector3.zero;
        isRolling = false; // 구르기 종료
    }

    public bool IsRolling => isRolling; // 구르기 상태를 외부에서 확인
}

