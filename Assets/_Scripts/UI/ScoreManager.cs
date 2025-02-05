using UnityEngine;
using TMPro;
using System.Collections;
using UnityEngine.SocialPlatforms.Impl;
public class ScoreManager : MonoBehaviour
{
    [SerializeField] TMP_Text ScoreLabel;
    [SerializeField] TMP_Text currentTimeText;

    private void Start()
    {
        float savedScoreFloat = PlayerPrefs.GetFloat("currentScore", 0);
        string currentScoreString = savedScoreFloat.ToString("#.###");

        currentTimeText.text = currentScoreString;

        string[] scores = PlayerPrefs.GetString("HighScores", "").Split(',');
        string result = "";

        for(int i = 0; i < scores.Length; i++)
        {
            result += (i + 1) + ". " + scores[i] + "\n";
        }

        ScoreLabel.text = result;
    }

}
