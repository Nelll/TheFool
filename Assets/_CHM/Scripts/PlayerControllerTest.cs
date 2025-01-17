using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerControllerTest : MonoBehaviour
{
    [Header("Velocity")]
    [SerializeField] private float walkSpeed    = 7;    // �ȱ� �ӵ�
    [SerializeField] private float runSpeed     = 12;   // �޸��� �ӵ�
    [SerializeField] private float jumpSpeed    = 10;   // ���� �ӵ�
    [SerializeField] private float Gravity      = 10;   // �߷�
    [SerializeField] private float terminalSpeed = 20;  // ���ܼӵ�

    [Header("Camera")]
    [SerializeField] private Transform cameraHorizontal;
    [SerializeField] private Transform cameraVertical;
    [SerializeField] private float mouseSens    = 1;    // ���콺 ����
    [SerializeField] private float verticalAngleMin = -89f; // ī�޶� ���� ���� ���� Min ��
    [SerializeField] private float verticalAngleMax = 89f;  // ī�޶� ���� ���� ���� Max ��

    // ������Ʈ
    Rigidbody rb;
    CharacterController characterController;
    AnimationState aniState;

    // ����
    float horizontalAngle;  // ���� ���� (ī�޶� ����)
    float verticalAngle;    // ���� ���� (ī�޶� ����)
    float verticalSpeed;    // ���� �ӵ�
    bool isGrounded;        // ���� �ν�
    float groundedTimer;    // ���鿡 �ð�




    private void Start()
    {
        #region �÷��̾� ������Ʈ
        rb                  = GetComponent<Rigidbody>();
        characterController = GetComponent<CharacterController>();
        aniState            = GetComponent<AnimationState>();
        #endregion

        #region ���콺 Ŀ�� ����
        Cursor.lockState    = CursorLockMode.Locked; // ���콺 ���
        Cursor.visible      = false; // ���콺 ����
        #endregion

        #region �ʱ�ȭ
        horizontalAngle = transform.localEulerAngles.y; // ���� �ٶ󺸴� ����
        verticalAngle   = 0;                              // ���� �ٶ󺸴� ����
        verticalSpeed   = 0;
        isGrounded      = true;
        groundedTimer   = 0;
        aniState.IdleAction();
        #endregion
    }


    private void Update()
    {
        #region �̵�
        //Vector2 moveVector = moveAction.ReadValue<Vector2>();
        //Vector3 move = new Vector3(moveVector.x, 0, moveVector.y);    // ����

        float HInput = Input.GetAxis("Horizontal");
        float VInput = Input.GetAxis("Vertical");

        Vector3 move = new Vector3(HInput, 0, VInput);

        // �̵� ���Ͱ� 1���� ũ�� 1�� �����ݴϴ�.
        // �̵� �ӵ��� ������ �ָ� �ȵǱ� ������
        if (move.magnitude > 1)
        {
            move.Normalize();
            //aniState.WalkAction();
        }

        move = move * walkSpeed * Time.deltaTime;   // �̵�
        move = transform.TransformDirection(move);  // ���ð��������� ���͸� ������������� ���ͷ� ��ȯ
        if(move.magnitude > 0)
        {
            aniState.WalkAction();
        }
        else
        {
            aniState.IdleAction();
        }
        characterController.Move(move);             // move ����� �����Դϴ�.

        #endregion

        #region ī�޶� ����
        //Vector2 look = lookAction.ReadValue<Vector2>(); // Look �׼� ���͸� �����ɴϴ�.

        //// �¿� ���콺 �̵��� ī�޶� �����Դϴ�.
        //float turnCamY = look.x * mouseSens;
        //horizontalAngle += turnCamY;
        //if (horizontalAngle >= 360) horizontalAngle -= 360;
        //if (horizontalAngle < 0) horizontalAngle += 360;

        //// ��ȭ�� ���� �¿� ������ ����
        //Vector3 currentAngle = cameraHorizontal.localEulerAngles;
        //currentAngle.y = horizontalAngle;
        //cameraHorizontal.localEulerAngles = currentAngle;  // ī�޶� �¿� ȸ��

        //// ���� ���콺 �̵��� ī�޶� �����Դϴ�.
        //float turnCamX = look.y * mouseSens;
        //verticalAngle -= turnCamX;
        //verticalAngle = Mathf.Clamp(verticalAngle, verticalAngleMin, verticalAngleMax); // ���� �̵� ���� ����
        //Camera cam = Camera.main;
        //cam.fieldOfView -= turnCamX;
        //cam.fieldOfView = Mathf.Clamp(cam.fieldOfView, 30, 90);

        //// ��ȭ�� ���� ���� ������ ����
        //currentAngle = cameraVertical.localEulerAngles;
        //currentAngle.x = verticalAngle;
        //cameraVertical.localEulerAngles = currentAngle; // ī�޶� ���� ȸ��
        #endregion

        // ����
        //verticalSpeed -= Gravity * Time.deltaTime;
        //if (verticalSpeed < -terminalSpeed)
        //{
        //    verticalSpeed = terminalSpeed;
        //}
        //Vector2 verticalMove = new Vector3(0, verticalSpeed, 0);
        //verticalMove *= Time.deltaTime; // �Ʒ� �̵�
        //// CollisionFlags�� ���� Move �Լ��� ��� ���� ����
        //CollisionFlags flag = characterController.Move(verticalMove);
        //if ((flag & CollisionFlags.Below) != 0)
        //{
        //    // ���鿡 �浹�� ��
        //    verticalSpeed = 0;
        //    //aniState.LandingAction();
        //}
        //if ((flag & CollisionFlags.Above) != 0)
        //{
        //    // õ�忡 �浹�� ��
        //    verticalSpeed = 0;
        //}

        //// ���� �پ��ִ��� Ȯ��
        //Debug.Log(characterController.isGrounded);

        //if(!characterController.isGrounded)
        //{
        //    // ���鿡�� 0.3�� �Ѱ� ������������ isGrounded�� false�� �˴ϴ�.
        //    if(isGrounded)
        //    {
        //        groundedTimer += Time.deltaTime;
        //        if(groundedTimer > 0.3f)
        //        {
        //            isGrounded = false;
        //        }
        //        //aniState.FallAction();
        //    }
        //}
        //else
        //{
        //    // ���鿡 ������ �ʱ�ȭ
        //    isGrounded = true;
        //    groundedTimer = 0;
        //    //aniState.IdleAction();
        //}
    }

    // Player Input ������Ʈ�� �����ϴ� �Լ�
    // Space Bar�� �۵�
    void OnJump()
    {
        if(isGrounded)
        {
            verticalSpeed = jumpSpeed;
            //aniState.JumpAction();
            isGrounded = false;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("HitBox"))
        {
            Debug.Log("�¾Ҽ���");
        }
    }
}
