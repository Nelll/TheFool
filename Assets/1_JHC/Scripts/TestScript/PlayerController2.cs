using System;
using System.Collections;
using System.Runtime.InteropServices;
using UnityEngine;

public class PlayerController2 : MonoBehaviour
{
    [SerializeField] private Transform cameraTransform; // 카메라 Transform, 이동 방향 계산에 사용

    [Header("데미지 관련")]
    [SerializeField] private Status1 playerStatus;
    [SerializeField] private DamageManager damageManager;

    [Header("스킬 쿨타임")]
    [SerializeField] private SkillUIManager skillUIManager;
    [SerializeField] private float ultimateCooldown = 5.0f;  // 궁극기 쿨타임 (초 단위)

    public TrailRenderer trailEffect;
    private PlayerMovement playerMovement;
    private PlayerAnimator playerAnimator;

    // * 추가한 부분
    private bool initialClickBlocked = false;
    private bool isSoundEnabled = false;

    private bool isUltimateOnCooldown = false;

    private void Awake()
    {
        Cursor.visible = false; // 마우스 커서 보이지 않게
        Cursor.lockState = CursorLockMode.Locked;   // 마우스 커서 위치 고정

        playerMovement = GetComponent<PlayerMovement>();
        playerAnimator = GetComponentInChildren<PlayerAnimator>();

        // *(추가한 부분) 게임 시작 시 마우스 버튼이 눌린 경우 방지
        if (Input.GetMouseButton(0))
        {
            initialClickBlocked = true;
        }
    }

    // * 추가한 부분
    private IEnumerator Start()
    {
        yield return new WaitForSeconds(1.0f); // 1초 후 활성화
        isSoundEnabled = true;
    }

    private void Update()
    {
        // 구르기 상태를 확인하여 다른 입력을 제한
        if (playerMovement.IsRolling) return;

        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        // 입력 벡터를 카메라 기준으로 변환
        Vector3 cameraForward = cameraTransform.forward;
        Vector3 cameraRight = cameraTransform.right;

        // 정규화하여 방향 벡터 계산
        cameraForward.Normalize();
        cameraRight.Normalize();

        // y축 이동 방지 (카메라의 수직 방향 제거)
        cameraForward.y = 0;
        cameraRight.y = 0;

        // 입력에 따라 이동 방향 결정
        Vector3 direction = cameraForward * z + cameraRight * x;

        // 이동 여부, 달리기 상태 체크
        bool isWalking = direction.magnitude > 0;                       // 입력 벡터의 크기가 0보타 크면 이동
        bool isRunning = Input.GetKey(KeyCode.LeftShift) && isWalking;

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

        if (Input.GetMouseButtonDown(0))
        {
            Debug.Log("공격 클릭 완료");
            playerAnimator.OnWeaponAttack();
            trailEffect.enabled = true;
            damageManager.Damage = playerStatus.Attack;  // 기본 공격 데미지 적용
        }

        if (Input.GetKeyDown(KeyCode.R) && !isUltimateOnCooldown)
        {
            StartCoroutine(HandleUltimateCooldown());
            Debug.Log("궁극기");
            playerAnimator.OnUltimateAttack();
            trailEffect.enabled = true;
            damageManager.Damage = playerStatus.UltimateAttackDamage;  // 궁극기 공격 데미지 적용
        }

        if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            playerAnimator.OnRoll();
            playerMovement.Roll();
        }
    }

    // 궁극기 쿨타임 처리 코루틴
    private IEnumerator HandleUltimateCooldown()
    {
        isUltimateOnCooldown = true;
        UpdateUltimateCooldownUI(true);  // UI 업데이트 (쿨타임 시작)
        yield return new WaitForSeconds(ultimateCooldown);
        isUltimateOnCooldown = false;
        UpdateUltimateCooldownUI(false);  // UI 업데이트 (쿨타임 종료)
    }

    // 쿨타임 UI 업데이트 메서드 (UIManager를 통해 처리)
    private void UpdateUltimateCooldownUI(bool isCooldownActive)
    {
        if (skillUIManager != null)
        {
            skillUIManager.SetUltimateCooldown(isCooldownActive);
        }
    }

    public void EnableTrail()
    {
        trailEffect.enabled = true;
    }

    public void DisableTrail()
    {
        trailEffect.enabled = false;
    }

    public void PlayFootstepSound() => PlayerSoundManager.Instance.PlayFootstepSound();
    public void PlayRunstepSound() => PlayerSoundManager.Instance.PlayRunstepSound();
    public void PlayAttackSound1() => PlayerSoundManager.Instance.PlayAttackSound1();
    public void PlayAttackSound2() => PlayerSoundManager.Instance.PlayAttackSound2();
    public void PlayAttackSound3() => PlayerSoundManager.Instance.PlayAttackSound3();
    public void PlayAttackSound4() => PlayerSoundManager.Instance.PlayAttackSound4();
    public void PlayRollSound() => PlayerSoundManager.Instance.PlayRollSound();
}