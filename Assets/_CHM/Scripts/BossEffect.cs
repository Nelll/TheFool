using UnityEngine;

public class BossEffect : MonoBehaviour
{
    [SerializeField] private GameObject[] breathEffects; // 브레스 파티클
    [SerializeField] private GameObject[] breathLight;      // 브레스 파티클 제외 기타 요소

    [SerializeField] private GameObject clawEffect;
    [SerializeField] private GameObject biteEffect;
    [SerializeField] private GameObject FlyAttackEffect;


    public void StartBreatheParticle()
    {
        for (int l = 0; l < breathLight.Length; l++)
        {
            breathLight[l].SetActive(true);
        }
        for (int p = 0; p < breathEffects.Length; p++)
        {
            breathEffects[p].GetComponent<ParticleSystem>().Play();
        }
    }

    public void StopBreatheParticle()
    {
        for (int l = 0; l < breathLight.Length; l++)
        {
            breathLight[l].SetActive(false);
        }
        for (int p = 0; p < breathEffects.Length; p++)
        {
            breathEffects[p].GetComponent<ParticleSystem>().Stop();
        }
    }

    public void StartClawEffect()
    {
        clawEffect.SetActive(true);
    }

    public void StopClawEffect()
    {
        clawEffect.SetActive(false);
    }

    public void StartBiteEffect()
    {
        biteEffect.SetActive(true);
    }

    public void StopBiteEffect()
    {
        biteEffect.SetActive(false);
    }
    public void StartFlyAttackEffect()
    {
        FlyAttackEffect.SetActive(true);
    }

    public void StopFlyAttackEffect()
    {
        FlyAttackEffect.SetActive(false);
    }

}
