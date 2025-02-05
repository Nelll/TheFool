using UnityEditor;
using UnityEngine;

public class MPDamageManager : MonoBehaviour
{
    [SerializeField] Material material;
    MentalPoint mentalPoint;
    float time = 0;
    private void Awake()
    {
        material.SetFloat("_VignetteIntensity", 0);
        mentalPoint = GetComponent<MentalPoint>();
    }

    private void Update()
    {
        time += Time.deltaTime;
        if(time >1)
        {
            time = 0;
            mentalPoint.TakeDamage(1);
        }

        if(mentalPoint.currentMentalPoint <= mentalPoint.maxMentalPoint/2)
        {
            if(mentalPoint.currentMentalPoint <= mentalPoint.maxMentalPoint / 3)
            {
                material.SetFloat("_VignetteIntensity", 4);
            }
            else
            {
                material.SetFloat("_VignetteIntensity", 2);
            }

        }
        else
        {
            material.SetFloat("_VignetteIntensity", 0);
        }
    }
}
