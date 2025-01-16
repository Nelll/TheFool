using System.Collections;
using System.Runtime.InteropServices;
using TreeEditor;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private Transform cameraTransform; // 카메라 Transform, 이동 방향 계산에 사용
    private PlayerMovement playerMovement;
    private PlayerAnimator playerAnimator;

    private void Awake()
    {
        Cursor.visible = false; // 마우스 커서 보이지 않게
        Cursor.lockState = CursorLockMode.Locked;   // 마우스 커서 위치 고정

        playerMovement = GetComponent<PlayerMovement>();
        playerAnimator = GetComponentInChildren<PlayerAnimator>();
    }

    private void Update()
    {
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        // 입력 벡터를 카메라 기준으로 변환
        Vector3 cameraForward = cameraTransform.forward;
        Vector3 cameraRight = cameraTransform.right;

        // y축 이동 방지 (카메라의 수직 방향 제거)
        cameraForward.y = 0;
        cameraRight.y = 0;

        // 정규화하여 방향 벡터 계산
        cameraForward.Normalize();
        cameraRight.Normalize();

        // 입력에 따라 이동 방향 결정
        Vector3 direction = cameraForward * z + cameraRight * x;

        // 이동 여부, 달리기 상태 체크
        bool isWalking = direction.magnitude > 0;                       // 입력 벡터의 크기가 0보타 크면 이동
        bool isRunning = Input.GetKey(KeyCode.LeftShift) && isWalking;  // 

        // 이동 처리
        if (isWalking)
        {
            playerMovement.Move(direction);

            // 이동 방향으로 회전 처리
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 10.0f);
        }

        // 애니메이터 상태 업데이트
        playerAnimator.OnWalk(isWalking && !isRunning);     // 걷는중, 달리기 상태 아닐경우 OnWalk
        playerAnimator.OnRun(isRunning);                    // 달리는 중 OnRun

        if (Input.GetKey(KeyCode.Space))
        {
            playerAnimator.OnJump();    // 애니메이션 파라미터 설정(onJump)
            playerMovement.Jump();      // 점프 함수 호출
        }

        if (Input.GetMouseButton(0))
        {
            Debug.Log("클릭 완료");
            playerAnimator.OnWeaponAttack();
        }
    }
}
