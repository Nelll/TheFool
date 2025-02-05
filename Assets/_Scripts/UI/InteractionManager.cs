using UnityEngine;
using TMPro;

public class InteractionManager : MonoBehaviour
{
    [SerializeField] float maxDistance;
    [SerializeField] LayerMask layerMask;
    [SerializeField] GameObject interactionCanvas;
    [SerializeField] MPRecovery mpRecovery;

    private void Awake()
    {
        interactionCanvas.SetActive(false);
    }

    private void Update()
    {
        Ray ray = Camera.main.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2, 0));
        RaycastHit hit;

        if(Physics.Raycast(ray, out hit, maxDistance, layerMask) && mpRecovery.isRecovery == false)
        {
            interactionCanvas.SetActive(true);

        }
        else
        {
            interactionCanvas.SetActive(false);
        }
    }
}
