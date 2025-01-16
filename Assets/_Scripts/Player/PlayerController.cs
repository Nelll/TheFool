using System.Collections;
using System.Runtime.InteropServices;
using TreeEditor;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private Transform cameraTransform; // ī�޶� Transform, �̵� ���� ��꿡 ���
    private PlayerMovement playerMovement;
    private PlayerAnimator playerAnimator;

    private void Awake()
    {
        Cursor.visible = false; // ���콺 Ŀ�� ������ �ʰ�
        Cursor.lockState = CursorLockMode.Locked;   // ���콺 Ŀ�� ��ġ ����

        playerMovement = GetComponent<PlayerMovement>();
        playerAnimator = GetComponentInChildren<PlayerAnimator>();
    }

    private void Update()
    {
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        // �Է� ���͸� ī�޶� �������� ��ȯ
        Vector3 cameraForward = cameraTransform.forward;
        Vector3 cameraRight = cameraTransform.right;

        // y�� �̵� ���� (ī�޶��� ���� ���� ����)
        cameraForward.y = 0;
        cameraRight.y = 0;

        // ����ȭ�Ͽ� ���� ���� ���
        cameraForward.Normalize();
        cameraRight.Normalize();

        // �Է¿� ���� �̵� ���� ����
        Vector3 direction = cameraForward * z + cameraRight * x;

        // �̵� ����, �޸��� ���� üũ
        bool isWalking = direction.magnitude > 0;                       // �Է� ������ ũ�Ⱑ 0��Ÿ ũ�� �̵�
        bool isRunning = Input.GetKey(KeyCode.LeftShift) && isWalking;  // 

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

        if (Input.GetKey(KeyCode.Space))
        {
            playerAnimator.OnJump();    // �ִϸ��̼� �Ķ���� ����(onJump)
            playerMovement.Jump();      // ���� �Լ� ȣ��
        }

        if (Input.GetMouseButton(0))
        {
            Debug.Log("Ŭ�� �Ϸ�");
            playerAnimator.OnWeaponAttack();
        }
    }
}
