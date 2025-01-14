using System.Runtime.InteropServices;
using UnityEngine;

public class ComboManager : MonoBehaviour
{
    [SerializeField] private Animator anim;
    [SerializeField] private float combatDelay = 1.0f; // 콤보가 재설정되기 전 허용 시간
    private int numClicks = 0; // 클릭 수
    private float lastClickTime = 0f; // 마지막 클릭 시간
    private bool canInterrupt = false; // Attack3에서 바로 Attack1로 넘어갈 수 있는지 여부

    private void Update()
    {
        // 일정 시간이 경과하면 콤보 초기화
        if (Time.time - lastClickTime > combatDelay)
        {
            ResetCombo();
        }

        // 왼쪽 마우스 버튼 입력 처리
        if (Input.GetMouseButtonDown(0))
        {
            lastClickTime = Time.time; // 마지막 클릭 시간 갱신

            if (canInterrupt && numClicks == 3)
            {
                // Attack3 상태에서 바로 Attack1로 시작 가능
                numClicks = 1;
            }
            else
            {
                numClicks++;
                numClicks = Mathf.Clamp(numClicks, 1, 3); // 클릭 수 제한
            }

            TriggerComboAnimation();
        }
    }

    private void TriggerComboAnimation()
    {
        switch (numClicks)
        {
            case 1:
                anim.SetTrigger("Attack1");
                break;
            case 2:
                anim.SetTrigger("Attack2");
                break;
            case 3:
                anim.SetTrigger("Attack3");
                break;
        }

        // Attack3 상태에서는 바로 리셋 가능
        canInterrupt = (numClicks == 3);
    }

    private void ResetCombo()
    {
        numClicks = 0; // 클릭 수 초기화
        canInterrupt = false; // 중단 가능 상태 초기화
    }
}
