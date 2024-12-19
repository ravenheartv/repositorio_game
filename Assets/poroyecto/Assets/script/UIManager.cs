using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public Text scoreText;
    public Text missedText;
    public GameObject gameOverPanel;

    public void UpdateScore(int score)
    {
        scoreText.text = $"Score: {score}";
    }

    public void UpdateMissed(int missed, int maxMissed)
    {
        missedText.text = $"Missed: {missed}/{maxMissed}";
    }

    public void ShowGameOver()
    {
        gameOverPanel.SetActive(true);
    }
}
