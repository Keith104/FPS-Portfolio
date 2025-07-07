using TMPro;
using UnityEngine;

public class Gamemanager : MonoBehaviour
{
    public static Gamemanager instance;

    [SerializeField] GameObject menuActive;


    [Header("UI References")]
    [SerializeField] GameObject menuPause;


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
        if(Input.GetButtonDown("Cancel"))
        {
            if(menuActive == null)
            {
                statePause();
                menuActive = menuPause;
                menuActive.SetActive(true);
            }
            else if (menuActive == menuPause)
            {
                stateUnPause();
            }
        }

        timer += Time.deltaTime;
        UpdateTimerText();
    }

    public void UpdateScoreText(int amount)
    {
        score += amount;
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

        //timerText.text = $"Timer: {formatted}";
    }

    public void statePause()
    {
        isPaused = !isPaused;
        Time.timeScale = 0f;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    public void stateUnPause()
    {
        isPaused = !isPaused;
        Time.timeScale = timeScaleOrig;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        menuActive.SetActive(false);
        menuActive = null;
    }
}
