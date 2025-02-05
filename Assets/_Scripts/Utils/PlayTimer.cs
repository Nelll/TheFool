using UnityEngine;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
public class PlayTimer : MonoBehaviour
{
    private void Update()
    {
        if(GameManager.Instance.isCleared == true)
        {
            Time.timeScale = 0;
            SaveHighScore();
            GameManager.Instance.isCleared = false;
            SceneManager.LoadScene("GameWinScene");
        }
    }

    void SaveHighScore()
    {
        float score = GameManager.Instance.playTime;    // ���� ����

        PlayerPrefs.SetFloat("currentScore", score);

        string currentScoreString = score.ToString("#.###");    // ���� ���ڿ� ��ȯ
        string savedScoreString = PlayerPrefs.GetString("HighScores", "");  // �⺻ �� ����
        if(savedScoreString == "")
        {
            PlayerPrefs.SetString("HighScores", currentScoreString);
        }
        else
        {
            string[] scoreArray = savedScoreString.Split(','); // ����� ���� �и�
            List<string> scoreList = new List<string>(scoreArray);
            for(int i = 0; i < scoreList.Count; i++) // ������ ��ġ�� �� ���ھ� �ֱ�
            {
                Debug.Log("for �� i : " + i + " = " + scoreList[i]);
                if (scoreList[i] == "")
                {
                    scoreList.RemoveAt(i);
                }
                Debug.Log("for �� i : " + i + " = " + scoreList[i]);
                float savedScore = float.Parse(scoreList[i]);
                Debug.Log("savedScore : " + savedScore);
                if (savedScore > score) // ����� ����(Ÿ��)�� ���� ����(Ÿ��)���� ���� ��� 
                {
                    scoreList.Insert(i, currentScoreString); // ���� ������ ���� ��ġ�� �ڸ��� �ٲ۴�.
                    break;
                }
            }

            if(scoreArray.Length == scoreList.Count) // ���� ������ ���� �� ���� ������ �� �ڷ�
            {
                scoreList.Add(currentScoreString);
            }

            if(scoreList.Count > 10) // ����� ������ 10���� ������ ���� �� ���� ���� ����
            {
                scoreList.RemoveAt(10);
            }

            string result = string.Join(",", scoreList); // ����Ʈ�� �ϳ��� ��Ʈ������ ��ġ��
            PlayerPrefs.SetString("HighScores", result);
        }

        PlayerPrefs.Save();
    }
}
