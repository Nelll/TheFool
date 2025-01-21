using UnityEngine;
using UnityEngine.UI;

public class StatDisplayer : MonoBehaviour
{
    [SerializeField] Health health;
    [SerializeField] Image healthBarImage;

    private void Update()
    {
        healthBarImage.fillAmount = health.currentHealth / (float)health.maxHealth;
    }

}
