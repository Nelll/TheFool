using UnityEngine;
using TMPro;
public class ScoreManager : MonoBehaviour
{
    [SerializeField] TMP_Text ScoreLabel;

    private void Start()
    {
        string[] scores = PlayerPrefs.GetString("HighScores", "").Split(',');
        string result = "";

        for(int i = 0; i < scores.Length; i++)
        {
            result += (i + 1) + ". " + scores[i] + "\n";
        }

        ScoreLabel.text = result;
    }
}
