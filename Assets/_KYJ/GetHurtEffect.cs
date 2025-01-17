using UnityEngine;
using System.Collections;

public class GetHurtEffect : MonoBehaviour
{
    /// <summary>
    /// 이 스크립트는 SkinnedMeshRenderer가 들어있는 오브젝트들의 부모 오브젝트에 넣으면 됩니다.
    /// 피격 이벤트에 StartCoroutine(GetHurt());를 호출시키면 됩니다.
    /// </summary>

    SkinnedMeshRenderer[] skinMeshes;
    public float hurtTime = 0.1f; // 데미지 입었을 때 반짝거릴 시간
    float waitTime = 0.5f; // 이펙트 반복을 막기 위한 임의 시간 할당

    void Awake()
    {
        skinMeshes = GetComponentsInChildren<SkinnedMeshRenderer>();
    }

    IEnumerator GetHurt()
    {
        foreach (SkinnedMeshRenderer meshes in skinMeshes)
        {
            meshes.material.color = Color.red; // 일단 MeshRenderer의 색상을 빨갛게 했습니다
        }

        yield return new WaitForSeconds(hurtTime);

        foreach (SkinnedMeshRenderer meshes in skinMeshes)
        {
            meshes.material.color = Color.white; // 원상 복구
        }

        yield return new WaitForSeconds(waitTime);
    }
}