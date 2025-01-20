using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5.0f;    // �ӵ�
    [SerializeField] private float runSpeed = 8.0f;     // �޸��� �ӵ�

    [SerializeField] private float rollSpeed = 10.0f;   // ������ �ӵ�
    [SerializeField] private float rollDuration = 1.05f; // ������ �ð�

    [SerializeField] private float jumpPower = 3.0f;    // ����
    private float gravity = -9.81f;                     // �߷�
    private Vector3 moveDirection;                      // �̵�����

    private CharacterController characterController;
    private bool isRolling = false; // ������ ���� ����
    private bool isRunning = false;                     // �޸��� ���� ����

    private void Awake()
    {
        characterController = GetComponent<CharacterController>();
    }

    private void Update()
    {
        // �߷� ����, ĳ���Ͱ� ���� ���� ���� ���¸� y�� ���⿡ gravity * Time.deltaTime�� ���� �߷� ����
        if (!characterController.isGrounded)
        {

            moveDirection.y += gravity * Time.deltaTime;
        }

        if (!isRolling)
        {
            // ���� �̵� �ӵ�: �޸��� ���¶�� runSpeed, �׷��� ������ moveSpeed ��� (�޸��� ���¿� ���� �ӵ� ����)
            float currentSpeed = isRunning ? runSpeed : moveSpeed;

            // �̵� ó��: ����� �ӵ��� ������� ĳ���͸� �̵�
            characterController.Move(moveDirection * moveSpeed * Time.deltaTime);
        }
    }

    // �ܺο��� ȣ���ؼ� �Ű������� ���������� ��������, ���� ������ moveDirection�� ����
    public void Move(Vector3 direction)
    {
        if (!isRolling)
        {
            // �Է¹��� ������ moveDirection�� ����
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
        gravity = 0; // �߷� ��Ȱ��ȭ
        yield return new WaitForSeconds(0.2f); // ���� �ð� ���
        gravity = gravityBackup; // �߷� ����
    }

    // �޸��� ���¸� ����
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
        isRolling = true; // ������ ����
        Vector3 rollDirection = transform.forward; // ������ ����
        moveDirection = Vector3.zero;

        float elapsedTime = 0f;
        while (elapsedTime < rollDuration)
        {
            characterController.Move(rollDirection * rollSpeed * Time.deltaTime); // ������ �̵�
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        moveDirection = Vector3.zero;
        isRolling = false; // ������ ����
    }

    public bool IsRolling => isRolling; // ������ ���¸� �ܺο��� Ȯ��
}

