using System.Collections;
using UnityEngine;

public class PlayerAttackCollision : MonoBehaviour
{
    // AttackCollision Ȱ��ȭ �Ǹ� �ڷ�ƾ ����
    private void OnEnable()
    {
        StartCoroutine("AutoDisable");
    }

    private void OnTriggerEnter(Collider other)
    {
        // �÷��̾ Ÿ���ϴ� ����� �±�, ������Ʈ, �Լ��� �ٲ� �� �ִ�
        if (other.CompareTag("Enemy"))
        {
            Debug.Log("Hit Enemy");
            // �浹�ϴ� ���� �±װ� Enemy�̸�, �������� �ֵ��� �߰� ����

        }
    }

    private IEnumerator AutoDisable()
    {
        // 0.1�� �Ŀ� AttackCollision������Ʈ�� ��������� �Ѵ�
        yield return new WaitForSeconds(0.1f);

        gameObject.SetActive(false);
    }
}
