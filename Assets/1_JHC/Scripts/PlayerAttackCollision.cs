using System.Collections;
using UnityEngine;

public class PlayerAttackCollision : MonoBehaviour
{
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
        }
    }

    private IEnumerator AutoDisable()
    {
        // 0.1�� �Ŀ� ������Ʈ�� ��������� �Ѵ�
        yield return new WaitForSeconds(0.1f);

        gameObject.SetActive(false);
    }
}
