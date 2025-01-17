using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerControllerTest : MonoBehaviour
{
    [Header("Velocity")]
    [SerializeField] private float walkSpeed    = 7;    // 걷기 속도
    [SerializeField] private float runSpeed     = 12;   // 달리기 속도
    [SerializeField] private float jumpSpeed    = 10;   // 점프 속도
    [SerializeField] private float Gravity      = 10;   // 중력
    [SerializeField] private float terminalSpeed = 20;  // 종단속도

    [Header("Camera")]
    [SerializeField] private Transform cameraHorizontal;
    [SerializeField] private Transform cameraVertical;
    [SerializeField] private float mouseSens    = 1;    // 마우스 감도
    [SerializeField] private float verticalAngleMin = -89f; // 카메라 시점 상하 범위 Min 값
    [SerializeField] private float verticalAngleMax = 89f;  // 카메라 시점 상하 범위 Max 값

    // 컴포넌트
    Rigidbody rb;
    CharacterController characterController;
    AnimationState aniState;

    // 변수
    float horizontalAngle;  // 수평 각도 (카메라 시점)
    float verticalAngle;    // 수직 각도 (카메라 시점)
    float verticalSpeed;    // 낙하 속도
    bool isGrounded;        // 지면 인식
    float groundedTimer;    // 지면에 시간




    private void Start()
    {
        #region 플레이어 컴포넌트
        rb                  = GetComponent<Rigidbody>();
        characterController = GetComponent<CharacterController>();
        aniState            = GetComponent<AnimationState>();
        #endregion

        #region 마우스 커서 설정
        Cursor.lockState    = CursorLockMode.Locked; // 마우스 잠금
        Cursor.visible      = false; // 마우스 숨김
        #endregion

        #region 초기화
        horizontalAngle = transform.localEulerAngles.y; // 현재 바라보는 방향
        verticalAngle   = 0;                              // 현재 바라보는 방향
        verticalSpeed   = 0;
        isGrounded      = true;
        groundedTimer   = 0;
        aniState.IdleAction();
        #endregion
    }


    private void Update()
    {
        #region 이동
        //Vector2 moveVector = moveAction.ReadValue<Vector2>();
        //Vector3 move = new Vector3(moveVector.x, 0, moveVector.y);    // 방향

        float HInput = Input.GetAxis("Horizontal");
        float VInput = Input.GetAxis("Vertical");

        Vector3 move = new Vector3(HInput, 0, VInput);

        // 이동 벡터가 1보다 크면 1로 맞춰줍니다.
        // 이동 속도에 영향을 주면 안되기 때문에
        if (move.magnitude > 1)
        {
            move.Normalize();
            //aniState.WalkAction();
        }

        move = move * walkSpeed * Time.deltaTime;   // 이동
        move = transform.TransformDirection(move);  // 로컬공간에서의 벡터를 월드공간에서의 벡터로 변환
        if(move.magnitude > 0)
        {
            aniState.WalkAction();
        }
        else
        {
            aniState.IdleAction();
        }
        characterController.Move(move);             // move 값대로 움직입니다.

        #endregion

        #region 카메라 시점
        //Vector2 look = lookAction.ReadValue<Vector2>(); // Look 액션 벡터를 가져옵니다.

        //// 좌우 마우스 이동은 카메라만 움직입니다.
        //float turnCamY = look.x * mouseSens;
        //horizontalAngle += turnCamY;
        //if (horizontalAngle >= 360) horizontalAngle -= 360;
        //if (horizontalAngle < 0) horizontalAngle += 360;

        //// 변화된 현재 좌우 각도를 적용
        //Vector3 currentAngle = cameraHorizontal.localEulerAngles;
        //currentAngle.y = horizontalAngle;
        //cameraHorizontal.localEulerAngles = currentAngle;  // 카메라 좌우 회전

        //// 상하 마우스 이동은 카메라만 움직입니다.
        //float turnCamX = look.y * mouseSens;
        //verticalAngle -= turnCamX;
        //verticalAngle = Mathf.Clamp(verticalAngle, verticalAngleMin, verticalAngleMax); // 상하 이동 범위 설정
        //Camera cam = Camera.main;
        //cam.fieldOfView -= turnCamX;
        //cam.fieldOfView = Mathf.Clamp(cam.fieldOfView, 30, 90);

        //// 변화된 현재 상하 각도를 적용
        //currentAngle = cameraVertical.localEulerAngles;
        //currentAngle.x = verticalAngle;
        //cameraVertical.localEulerAngles = currentAngle; // 카메라 상하 회전
        #endregion

        // 낙하
        //verticalSpeed -= Gravity * Time.deltaTime;
        //if (verticalSpeed < -terminalSpeed)
        //{
        //    verticalSpeed = terminalSpeed;
        //}
        //Vector2 verticalMove = new Vector3(0, verticalSpeed, 0);
        //verticalMove *= Time.deltaTime; // 아래 이동
        //// CollisionFlags를 통해 Move 함수의 결과 값을 리턴
        //CollisionFlags flag = characterController.Move(verticalMove);
        //if ((flag & CollisionFlags.Below) != 0)
        //{
        //    // 지면에 충돌할 때
        //    verticalSpeed = 0;
        //    //aniState.LandingAction();
        //}
        //if ((flag & CollisionFlags.Above) != 0)
        //{
        //    // 천장에 충돌할 때
        //    verticalSpeed = 0;
        //}

        //// 땅에 붙어있는지 확인
        //Debug.Log(characterController.isGrounded);

        //if(!characterController.isGrounded)
        //{
        //    // 지면에서 0.3초 넘게 떨어져있으면 isGrounded는 false가 됩니다.
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
        //    // 지면에 있으면 초기화
        //    isGrounded = true;
        //    groundedTimer = 0;
        //    //aniState.IdleAction();
        //}
    }

    // Player Input 컴포넌트가 지원하는 함수
    // Space Bar로 작동
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
            Debug.Log("맞았수다");
        }
    }
}
