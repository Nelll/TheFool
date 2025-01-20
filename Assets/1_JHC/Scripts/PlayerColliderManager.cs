using UnityEngine;

public class PlayerColliderManager : MonoBehaviour
{
    [SerializeField] private CapsuleCollider weaponCollider;

    private void Start()
    {
        if (weaponCollider != null)
        {
            weaponCollider.enabled = false;  // 기본적으로 꺼둠
        }
    }

    // 애니메이션 이벤트에서 호출할 메서드 (공격 시작 시)
    public void EnableWeaponCollider()
    {
        if (weaponCollider != null)
        {
            weaponCollider.enabled = true;
            Debug.Log("무기 활성화");
        }
    }

    // 애니메이션 이벤트에서 호출할 메서드 (공격 종료 시)
    public void DisableWeaponCollider()
    {
        if (weaponCollider != null)
        {
            weaponCollider.enabled = false;
            Debug.Log("무기 비활성화");
        }
    }
}
