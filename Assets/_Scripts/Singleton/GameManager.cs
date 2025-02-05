using UnityEngine;
using UnityEngine.SceneManagement;

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

    GameObject target;

    public float playTime = 0;

    private bool isCleared;

    public bool IsCleared {  get { return isCleared; } }

    private void Start()
    {
        isCleared = false;
    }

    private void Update()
    {
        if(SceneManager.GetActiveScene().name == "GameScene")
        {
            playTime += Time.deltaTime;
        }
    }

    public void LoadMenuScene()
    {
        SceneManager.LoadScene("MenuScene");
    }

    public void LoadGameScene()
    {
        SceneManager.LoadScene("GameScene");
    }
    public void LoadGameWin()
    {
        isCleared = true;
    }

    public void LoadGameOver()
    {
        SceneManager.LoadScene("GameOverScene");
    }

    public void OnPressedQuit()
    {
        Application.Quit();
    }
}
