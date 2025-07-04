using TMPro;
using UnityEngine;

public class Gamemanager : MonoBehaviour
{
    public static Gamemanager instance;

    [Header("UI References")]
    [SerializeField] TextMeshProUGUI timerText;
    [SerializeField] TextMeshProUGUI scoreText;


    float timer;

    bool isPaused;

    float timeScaleOrig;

    int score;

    private void Awake()
    {
        instance = this;

        timeScaleOrig = Time.timeScale;
        timer = 0;
    }

    private void Update()
    {
        if (isPaused)
            return;

        timer += Time.deltaTime;
        UpdateTimerText();
    }

    public void UpdateScoreText(int amount)
    {
        score += amount;

        scoreText.text = "Score: " + score;
    }

    void UpdateTimerText()
    {
        int totalSeconds = Mathf.FloorToInt(timer);
        int minutes = totalSeconds / 60;
        int seconds = totalSeconds % 60;

        string formatted;

        if(totalSeconds < 60)
        {
            formatted = seconds.ToString("0");
        }
        else if(totalSeconds < 600)
        {
            formatted = $"{minutes}:{seconds:00}";
        }
        else
        {
            formatted = $"{minutes:00}:{seconds:00}";
        }

        timerText.text = $"Timer: {formatted}";
    }

    
}
