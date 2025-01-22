using System;
using System.Collections;
using System.Runtime.InteropServices;
using TreeEditor;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private Transform cameraTransform; // ī�޶� Transform, �̵� ���� ��꿡 ���

    public TrailRenderer trailEffect;

    private PlayerMovement playerMovement;
    private PlayerAnimator playerAnimator;

    // * �߰��� �κ�
    private bool initialClickBlocked = false;   
    private bool isSoundEnabled = false;

    private void Awake()
    {
        Cursor.visible = false; // ���콺 Ŀ�� ������ �ʰ�
        Cursor.lockState = CursorLockMode.Locked;   // ���콺 Ŀ�� ��ġ ����

        playerMovement = GetComponent<PlayerMovement>();
        playerAnimator = GetComponentInChildren<PlayerAnimator>();

        // *(�߰��� �κ�) ���� ���� �� ���콺 ��ư�� ���� ��� ����
        if (Input.GetMouseButton(0))
        {
            initialClickBlocked = true;
        }
    }

    // * �߰��� �κ�
    private IEnumerator Start()
    {
        yield return new WaitForSeconds(1.0f); // 1�� �� Ȱ��ȭ
        isSoundEnabled = true;
    }

    private void Update()
    {
        // ������ ���¸� Ȯ���Ͽ� �ٸ� �Է��� ����
        if (playerMovement.IsRolling) return;

        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        // �Է� ���͸� ī�޶� �������� ��ȯ
        Vector3 cameraForward = cameraTransform.forward;
        Vector3 cameraRight = cameraTransform.right;

        // ����ȭ�Ͽ� ���� ���� ���
        cameraForward.Normalize();
        cameraRight.Normalize();

        // y�� �̵� ���� (ī�޶��� ���� ���� ����)
        cameraForward.y = 0;
        cameraRight.y = 0;

        // �Է¿� ���� �̵� ���� ����
        Vector3 direction = cameraForward * z + cameraRight * x;

        // �̵� ����, �޸��� ���� üũ
        bool isWalking = direction.magnitude > 0;                       // �Է� ������ ũ�Ⱑ 0��Ÿ ũ�� �̵�
        bool isRunning = Input.GetKey(KeyCode.LeftShift) && isWalking;

        // �̵� ó��
        if (isWalking)
        {
            playerMovement.Move(direction);

            // �̵� �������� ȸ�� ó��
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 10.0f);
        }

        // �ִϸ����� ���� ������Ʈ
        playerAnimator.OnWalk(isWalking && !isRunning);     // �ȴ���, �޸��� ���� �ƴҰ�� OnWalk
        playerAnimator.OnRun(isRunning);                    // �޸��� �� OnRun

        if (Input.GetMouseButtonDown(0))
        {
            Debug.Log("���� Ŭ�� �Ϸ�");
            playerAnimator.OnWeaponAttack();
            trailEffect.enabled = true;
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            Debug.Log("�ñر�");
            playerAnimator.OnUltimateAttack();
            trailEffect.enabled = true;
        }

        if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            playerAnimator.OnRoll();
            playerMovement.Roll();
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
