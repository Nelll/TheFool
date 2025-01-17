using UnityEngine;
using System.Collections;

public class GetHurtEffect : MonoBehaviour
{
    /// <summary>
    /// �� ��ũ��Ʈ�� SkinnedMeshRenderer�� ����ִ� ������Ʈ���� �θ� ������Ʈ�� ������ �˴ϴ�.
    /// �ǰ� �̺�Ʈ�� StartCoroutine(GetHurt());�� ȣ���Ű�� �˴ϴ�.
    /// </summary>

    SkinnedMeshRenderer[] skinMeshes;
    public float hurtTime = 0.1f; // ������ �Ծ��� �� ��¦�Ÿ� �ð�
    float waitTime = 0.5f; // ����Ʈ �ݺ��� ���� ���� ���� �ð� �Ҵ�

    void Awake()
    {
        skinMeshes = GetComponentsInChildren<SkinnedMeshRenderer>();
    }

    IEnumerator GetHurt()
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