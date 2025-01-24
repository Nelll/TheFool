using UnityEngine;
using System.Collections;

public class GetHurtEffect : MonoBehaviour
{
    /// <summary>
    /// �� ��ũ��Ʈ�� SkinnedMeshRenderer�� ����ִ� ������Ʈ���� �θ� ������Ʈ�� ������ �˴ϴ�.
    /// �ǰ� �̺�Ʈ�� StartCoroutine(GetHurt());�� ȣ���Ű�� �˴ϴ�.
    /// </summary>
    /// 
    public Health health;
    SkinnedMeshRenderer[] skinMeshes;
    float hurtTime = 0.2f; // ������ �Ծ��� �� ��¦�Ÿ� �ð�
    float waitTime = 0.5f; // ����Ʈ �ݺ��� ���� ���� ���� �ð� �Ҵ�
    int previousHealth;

    void Awake()
    {
        skinMeshes = GetComponentsInChildren<SkinnedMeshRenderer>();
    }

    void Start()
    {
        if (health != null)
        {
            previousHealth = health.currentHealth;
        }
    }

    void Update()
    {
        if (health == null)
        {
            return;
        }

        if (health.currentHealth < previousHealth)
        {
            StartCoroutine(GetHurt());
        }

        previousHealth = health.currentHealth;
    }

    public IEnumerator GetHurt()
    {
        foreach (SkinnedMeshRenderer meshes in skinMeshes)
        {
            meshes.material.color = Color.red; // �ϴ� MeshRenderer�� ������ ������ �߽��ϴ�
        }

        yield return new WaitForSeconds(hurtTime);

        foreach (SkinnedMeshRenderer meshes in skinMeshes)
        {
            meshes.material.color = Color.white; // ���� ����
        }

        yield return new WaitForSeconds(waitTime);
    }
}