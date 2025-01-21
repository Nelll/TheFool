using UnityEngine;

public class MentalPoint : MonoBehaviour
{
    public float maxMentalPoint;
    public float currentMentalPoint;

    public Status status;
    private void Awake()
    {
        maxMentalPoint = status.MentalPoint;
        currentMentalPoint = maxMentalPoint;
    }

    public void TakeDamage(float damage)
    {
        ModifyMentalPoint(-damage);
    }

    public void Restore(float mentalPoint)
    {
        ModifyMentalPoint(mentalPoint);
    }


    void ModifyMentalPoint(float value)
    {
        currentMentalPoint = Mathf.Clamp(currentMentalPoint + value, 0, maxMentalPoint);
    }
}
