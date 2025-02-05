using UnityEngine;

public class MenuManager : MonoBehaviour
{
    [SerializeField] AudioClip buttonSound;
    [SerializeField] GameObject commandPopUp;

    public void OnButtonSound()
    {
        GetComponent<AudioSource>().PlayOneShot(buttonSound);
    }

    public void OnCommandPopUp()
    {
        commandPopUp.SetActive(true);
    }

    public void OffCommandPopUp()
    {
        commandPopUp.SetActive(false);
    }

}
