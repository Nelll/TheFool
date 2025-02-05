using UnityEngine;

public class BossStatDisplayManager : MonoBehaviour
{

    [SerializeField] GameObject bossStatDisplay;

    private void Awake()
    {
        bossStatDisplay.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            bossStatDisplay.SetActive(true);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            bossStatDisplay.SetActive(false);
        }
    }
}
