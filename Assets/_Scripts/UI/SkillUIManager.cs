using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class SkillUIManager : MonoBehaviour
{
    [SerializeField] private Image ultimateCooldownImage;  // UI 이미지
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
            ultimateCooldownImage.fillAmount = cooldownTimer / cooldownTime;  // 남은 쿨타임 비율 조정
            yield return null;
        }

        ultimateCooldownImage.fillAmount = 0f;  // 쿨타임 종료 시 이미지 숨김
    }

    public void SetUltimateCooldown(bool isCooldownActive)
    {
        if (isCooldownActive)
        {
            StartUltimateCooldown(10.0f);  // 10초 쿨타임 시작
        }
        else
        {
            ultimateCooldownImage.fillAmount = 0f;  // 쿨타임 종료
        }
    }
}
