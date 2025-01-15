using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform objectToFollow;
    public float followSpeed = 10f;
    public float sensitivity = 100f;
    public float clampAngle = 70f;
    public LayerMask collisionLayers; // 충돌 검사에 사용할 레이어 마스크

    private float rotX;
    private float rotY;

    public Transform realCamera;
    private Vector3 dirNormalized;
    private float finalDistance;
    public float minDistance;
    public float maxDistance;
    public float smoothness = 10f;

    void Start()
    {
        Variables();
        CursorLock(true);
    }

    void Update()
    {
        CameraRotation();
        CusorLockExit();
    }

    void LateUpdate()
    {
        FollowTarget();
        CameraDistance();
    }

    private void Variables()
    {
        rotX = transform.localRotation.eulerAngles.x;
        rotY = transform.localRotation.eulerAngles.y;
        dirNormalized = realCamera.localPosition.normalized;
        finalDistance = realCamera.localPosition.magnitude;
    }

    private void CameraRotation()
    {
        rotX += -(Input.GetAxis("Mouse Y")) * sensitivity * Time.deltaTime;
        rotY += Input.GetAxis("Mouse X") * sensitivity * Time.deltaTime;
        rotX = Mathf.Clamp(rotX, -clampAngle, clampAngle);
        Quaternion rotation = Quaternion.Euler(rotX, rotY, 0);
        transform.rotation = rotation;
    }

    private void FollowTarget()
    {
        if (objectToFollow != null)
        {
            Vector3 targetPosition = objectToFollow.position;
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, followSpeed * Time.deltaTime);
        }
        else
        {
            Debug.LogWarning("objectToFollow가 설정되지 않았습니다.");
        }
    }

    private void CameraDistance()
    {
        Vector3 desiredCameraPos = transform.TransformPoint(dirNormalized * maxDistance);
        RaycastHit hit;

        if (Physics.Linecast(transform.position, desiredCameraPos, out hit, collisionLayers))
        {
            Debug.Log("충돌");
            finalDistance = Mathf.Clamp(hit.distance, minDistance, maxDistance);
        }
        else
        {
            Debug.Log("충돌X");
            finalDistance = maxDistance;
        }

        realCamera.localPosition = Vector3.Lerp(realCamera.localPosition, dirNormalized * finalDistance, Time.deltaTime * smoothness);
    }

    private void CursorLock(bool isLocked)
    {
        Cursor.lockState = isLocked ? CursorLockMode.Locked : CursorLockMode.None;
        Cursor.visible = !isLocked;
    }

    private void CusorLockExit()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            CursorLock(false);
        }
        else if (Input.GetMouseButtonDown(0) && Cursor.lockState != CursorLockMode.Locked)
        {
            CursorLock(true);
        }
    }
}
