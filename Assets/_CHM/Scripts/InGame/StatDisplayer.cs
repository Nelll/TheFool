using UnityEngine;
using UnityEngine.UI;

public class StatDisplayer : MonoBehaviour
{
    [SerializeField] Health health;
    [SerializeField] Image healthBarImage;


    private void Start()
    {
        healthBarImage.fillAmount = health.currentHealth / health.maxHealth;
    }
    private void Update()
    {
        healthBarImage.fillAmount = health.currentHealth / health.maxHealth;
    }

}
