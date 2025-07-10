using TMPro;
using UnityEngine;
using UnityEngine.InputSystem.DualShock;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [SerializeField] GameObject menuActive;
    [SerializeField] bool inGame;


    [Header("UI References")]
    [SerializeField] GameObject menuPause;
    [SerializeField] GameObject menuWin;
    [SerializeField] GameObject menuLose;
    [SerializeField] GameObject creditsMenu;
    [SerializeField] GameObject menuSettings;

    [Header("References")]
    [SerializeField] Animator animator;

    public int sens;

    float timer;

    bool isPaused;

    float timeScaleOrig;

    int score;

    int goalCount;

    private void Awake()
    {
        instance = this;

        timeScaleOrig = Time.timeScale;
        timer = 0;

        sens = PlayerPrefs.GetInt("sens", sens);
    }

    private void Update()
    {
        if (animator != null && menuActive != null)
        {
            var state = animator.GetCurrentAnimatorStateInfo(0);
            if (state.IsName("Credits") && state.normalizedTime >= 1f && !animator.IsInTransition(0))
            {
                endCredits();
            }
        }

        if (inGame)
        {
            if (Input.GetButtonDown("Cancel"))
            {
                if (menuActive == null)
                {
                    statePause();
                    menuActive = menuPause;
                    menuActive.SetActive(true);
                }
                else if (menuActive == menuPause)
                {
                    stateUnPause();
                }else if(menuActive == menuSettings)
                {
                    backToPauseMenu();
                }
            }

        }

        if (inGame)
        {
            timer += Time.deltaTime;
            UpdateTimerText();
        }

    }

    public void UpdateScoreText(int amount)
    {
        score += amount;

        if (score <= 0)
        {
            //Winner Winner Chicken Dinner
            statePause();
            menuActive = menuWin;
            menuActive.SetActive(true);
        }
    }

    void UpdateTimerText()
    {
        int totalSeconds = Mathf.FloorToInt(timer);
        int minutes = totalSeconds / 60;
        int seconds = totalSeconds % 60;

        string formatted;

        if (totalSeconds < 60)
        {
            formatted = seconds.ToString("0");
        }
        else if (totalSeconds < 600)
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

    public void backToPauseMenu()
    {
        menuActive.SetActive(false);
        menuActive = menuPause;
        menuPause.SetActive(true);
    }

    public void SetActiveMenu(GameObject activeMenu)
    {
        menuActive.SetActive(false);
        menuActive = activeMenu;
        menuActive.SetActive(true);
    }

    public void startCredits()
    {
        menuActive = creditsMenu;
        creditsMenu.SetActive(true);
        creditsMenu.GetComponent<Animator>().Play("Credits");
    }

    void endCredits()
    {
        menuActive.SetActive(false);
        menuActive = null;
    }

    public bool GetPause()
    {
        return isPaused;
    }

    public void Lose()
    {
        statePause();
        menuActive = menuLose;
        menuActive.SetActive(true);
    }

    public void SetSens(int s)
    {
        sens = s;
        PlayerPrefs.SetInt("sens", s);
        PlayerPrefs.Save();
    }
}
