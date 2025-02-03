using UnityEngine;
using UnityEngine.UI;

public class PlayerStatDisplayer : MonoBehaviour
{
    [SerializeField] Health health;
    [SerializeField] Image healthBarImage;
    [SerializeField] MentalPoint mentalPoint;
    [SerializeField] Image mentalPointBarImage;

    private void Update()
    {
        healthBarImage.fillAmount = health.currentHealth / (float)health.maxHealth;
        mentalPointBarImage.fillAmount = mentalPoint.currentMentalPoint / (float)mentalPoint.maxMentalPoint;
    }
}
