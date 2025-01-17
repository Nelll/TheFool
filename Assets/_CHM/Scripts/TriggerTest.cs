using UnityEngine;

public class TriggerTest : MonoBehaviour
{
    [SerializeField] private string name;
    [SerializeField] private float invincibilityTime = 2f;

    bool isIncibilityTime = false;
    float count;
    private void Update()
    {
        if(isIncibilityTime == true)
        {
            count += Time.deltaTime;
            if(count >= invincibilityTime)
            {
                isIncibilityTime = false;
                count = 0;
            }
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if(isIncibilityTime == false)
        {
            if (other.CompareTag("Player"))
            {
                Debug.Log(name + ": Player");
            }
            if (other.CompareTag("HitBox"))
            {
                Debug.Log(name + ": HitBox");
                isIncibilityTime = true;
            }
        }

    }

    //private void OnCollisionEnter(Collision collision)
    //{
    //    if(collision.collider.tag == "Player")
    //    {
    //        Debug.Log(name + ": Player");
    //    }
    //    if (collision.collider.tag == "HitBox")
    //    {
    //        Debug.Log(name + ": HitBox");
    //    }
    //}
}
