using UnityEngine;

public class PlayerColliderManager : MonoBehaviour
{
    [SerializeField] private CapsuleCollider weaponCollider;

    private void Start()
    {
        if (weaponCollider != null)
        {
            weaponCollider.enabled = false;  // �⺻������ ����
        }
    }

    // �ִϸ��̼� �̺�Ʈ���� ȣ���� �޼��� (���� ���� ��)
    public void EnableWeaponCollider()
    {
        if (weaponCollider != null)
        {
            weaponCollider.enabled = true;
            Debug.Log("���� Ȱ��ȭ");
        }
    }

    // �ִϸ��̼� �̺�Ʈ���� ȣ���� �޼��� (���� ���� ��)
    public void DisableWeaponCollider()
    {
        if (weaponCollider != null)
        {
            weaponCollider.enabled = false;
            Debug.Log("���� ��Ȱ��ȭ");
        }
    }
}
