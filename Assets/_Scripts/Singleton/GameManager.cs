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
                singletonObject.AddComponent<PlayTimer>();

                DontDestroyOnLoad(singletonObject);
            }
            return instance;
        }
    }

    public float playTime;
    public bool isCleared;

    private void Start()
    {
        playTime = 0;
        isCleared = false;
        Time.timeScale = 1f;
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
