using UnityEngine;
using UnityEngine.SceneManagement;

public class MentalPoint : MonoBehaviour
{
    public int maxMentalPoint;
    public int currentMentalPoint;

    public Status status;
    private void Awake()
    {
        maxMentalPoint = status.MentalPoint;
        currentMentalPoint = maxMentalPoint;
    }

    public void TakeDamage(int damage)
    {
        ModifyMentalPoint(-damage);
    }

    public void Restore(int mentalPoint)
    {
        ModifyMentalPoint(mentalPoint);
    }


    void ModifyMentalPoint(int value)
    {
        int newMentalPoint = currentMentalPoint + value;
        currentMentalPoint = Mathf.Clamp(newMentalPoint, 0, maxMentalPoint);

        if(currentMentalPoint <= 0)
        {
            GameManager.Instance.LoadGameOver();
        }
    }
}
