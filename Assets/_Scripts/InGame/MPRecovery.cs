using System.Collections;
using UnityEngine;

public class MPRecovery : MonoBehaviour
{
    [SerializeField] MentalPoint mentalPoint;
    [SerializeField] int recoveryPoint = 100;
    [SerializeField] AudioClip recoverySound;

    public bool isRecovery = false;
    bool isTrigger = false;
    private void Update()
    {
        if(isTrigger == true)
        {
            if (Input.GetKey(KeyCode.F))
            {
                GetComponent<AudioSource>().PlayOneShot(recoverySound);
                mentalPoint.Restore(recoveryPoint);
                isRecovery = true;
                StartCoroutine(recoveryActive());
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player") && isRecovery == false)
        {
            isTrigger = true;
        }
    }

    IEnumerator recoveryActive()
    {
        yield return new WaitForSeconds(1f);
        gameObject.SetActive(false);
    }
}
