using UnityEngine;
using System.Collections;

public class GetHurtEffect : MonoBehaviour
{
    /// <summary>
    /// 이 스크립트는 SkinnedMeshRenderer가 들어있는 오브젝트들의 부모 오브젝트에 넣으면 됩니다.
    /// 피격 이벤트에 StartCoroutine(GetHurt());를 호출시키면 됩니다.
    /// </summary>
    /// 
    public Health health;
    SkinnedMeshRenderer[] skinMeshes;
    float hurtTime = 0.2f; // 데미지 입었을 때 반짝거릴 시간
    float waitTime = 0.5f; // 이펙트 반복을 막기 위한 임의 시간 할당
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