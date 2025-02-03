using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class SkillUIManager : MonoBehaviour
{
    [SerializeField] private Image ultimateCooldownImage;  // UI �̹���
    private float cooldownTime;
    private float cooldownTimer;

    public void StartUltimateCooldown(float duration)
    {
        cooldownTime = duration;
        cooldownTimer = duration;
        ultimateCooldownImage.fillAmount = 1f;
        StartCoroutine(UltimateCooldownRoutine());
    }

    private IEnumerator UltimateCooldownRoutine()
    {
        while (cooldownTimer > 0)
        {
            cooldownTimer -= Time.deltaTime;
            ultimateCooldownImage.fillAmount = cooldownTimer / cooldownTime;  // ���� ��Ÿ�� ���� ����
            yield return null;
        }

        ultimateCooldownImage.fillAmount = 0f;  // ��Ÿ�� ���� �� �̹��� ����
    }

    public void SetUltimateCooldown(bool isCooldownActive)
    {
        if (isCooldownActive)
        {
            StartUltimateCooldown(10.0f);  // 10�� ��Ÿ�� ����
        }
        else
        {
            ultimateCooldownImage.fillAmount = 0f;  // ��Ÿ�� ����
        }
    }
}
