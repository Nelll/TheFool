using System.Runtime.InteropServices;
using UnityEngine;

public class ComboManager : MonoBehaviour
{
    [SerializeField] private Animator anim;
    [SerializeField] private float combatDelay = 1.0f; // �޺��� �缳���Ǳ� �� ��� �ð�
    private int numClicks = 0; // Ŭ�� ��
    private float lastClickTime = 0f; // ������ Ŭ�� �ð�
    private bool canInterrupt = false; // Attack3���� �ٷ� Attack1�� �Ѿ �� �ִ��� ����

    private void Update()
    {
        // ���� �ð��� ����ϸ� �޺� �ʱ�ȭ
        if (Time.time - lastClickTime > combatDelay)
        {
            ResetCombo();
        }

        // ���� ���콺 ��ư �Է� ó��
        if (Input.GetMouseButtonDown(0))
        {
            lastClickTime = Time.time; // ������ Ŭ�� �ð� ����

            if (canInterrupt && numClicks == 3)
            {
                // Attack3 ���¿��� �ٷ� Attack1�� ���� ����
                numClicks = 1;
            }
            else
            {
                numClicks++;
                numClicks = Mathf.Clamp(numClicks, 1, 3); // Ŭ�� �� ����
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

        // Attack3 ���¿����� �ٷ� ���� ����
        canInterrupt = (numClicks == 3);
    }

    private void ResetCombo()
    {
        numClicks = 0; // Ŭ�� �� �ʱ�ȭ
        canInterrupt = false; // �ߴ� ���� ���� �ʱ�ȭ
    }
}
