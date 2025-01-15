using UnityEngine;

public class TestCameraSetKYJ : MonoBehaviour
{
    public Camera cam;
    public Transform obj;

    void Start() => cam.transform.SetParent(obj);
}
