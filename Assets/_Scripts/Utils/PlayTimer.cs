using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine.SceneManagement;
public class PlayTimer : MonoBehaviour
{

    private void Update()
    {
        if (GameManager.Instance.IsCleared == true)
        {
            Time.timeScale = 0;
            SaveHighScore();
            SceneManager.LoadScene("GameWinScene");
        }
    }

    void SaveHighScore()
    {
        float score = GameManager.Instance.playTime;    // 현재 점수

        string currentScoreString = score.ToString("#.###");    // 점수 문자열 변환
        string savedScoreString = PlayerPrefs.GetString("HighScores", "");  // 기본 값 세팅

        if(savedScoreString == "")
        {
            PlayerPrefs.SetString("HighScores", currentScoreString);
        }
        else
        {
            string[] scoreArray = savedScoreString.Split(','); // 저장된 점수 분리
            List<string> scoreList = new List<string>(scoreArray);

            for(int i = 0; i < scoreList.Count; i++) // 적절한 위치에 새 스코어 넣기
            {
                float savedScore = float.Parse(scoreList[i]);
                if (savedScore > score) // 저장된 점수(타임)이 현재 점수(타임)보다 높을 경우 
                {
                    scoreList.Insert(i, currentScoreString); // 현재 점수가 들어온 위치로 자리를 바꾼다.
                    break;
                }
            }

            if(scoreArray.Length == scoreList.Count) // 현재 점수가 가장 안 좋은 점수면 맨 뒤로
            {
                scoreList.Add(currentScoreString);
            }

            if(scoreList.Count > 10) // 저장된 점수가 10개가 넘으면 가장 안 좋은 점수 삭제
            {
                scoreList.RemoveAt(10);
            }

            string result = string.Join(",", scoreList); // 리스트를 하나의 스트링으로 합치기
            PlayerPrefs.SetString("HighScores", result);
        }

        PlayerPrefs.Save();
    }
}
