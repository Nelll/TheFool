using UnityEngine;

public class GameManager : MonoBehaviour
{
    static GameManager instance;

    public static GameManager Instance
    {
        get
        {
            if (instance == null)
            {
                GameObject singletonObject = new GameObject("GameManager");
                instance = singletonObject.AddComponent<GameManager>();

                DontDestroyOnLoad(singletonObject);
            }
            return instance;
        }
    }
}
