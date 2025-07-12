using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [SerializeField] GameObject menuActive;
    [SerializeField] bool inGame;


    [Header("UI References")]
    [SerializeField] GameObject menuPause;
    [SerializeField] GameObject menuWin;
    [SerializeField] GameObject menuLose;
    [SerializeField] GameObject menuCredits;
    [SerializeField] GameObject menuSettings;
    [SerializeField] TMP_Text waveText;
    [SerializeField] TMP_Text enemyText;
    [SerializeField] TMP_Text totalTimerText;
    [SerializeField] TMP_Text waveTimerText;
    [SerializeField] TMP_Text totalScoreText;
    [SerializeField] TMP_Text waveScoreText;
    public Button respawnButton;


    [Header("References")]
    [SerializeField] Animator animator;

    [Header("RunTime References")]
    [SerializeField] GameObject player;
    public PlayerController playerController;



    public int sens;

    // Win Condtion stat variables
    float totalTimer;
    float waveTimer;
    int totalScore;
    int waveScore;

    bool isPaused;

    float timeScaleOrig;


    int goalCount;

    int tempQuality;
    bool tempVsync;

    bool isFadingOut;

    private void Awake()
    {
        instance = this;

        timeScaleOrig = Time.timeScale;
        totalTimer = 0;
        if(inGame)
        {
            player = GameObject.FindGameObjectWithTag("Player");
            playerController = player.GetComponent<PlayerController>();
        }

        if(menuCredits != null)
        {
            animator = menuCredits.GetComponent<Animator>();
        }

        sens = PlayerPrefs.GetInt("sens", sens);

        tempQuality = Mathf.Clamp(
        PlayerPrefs.GetInt("quality", QualitySettings.GetQualityLevel()),
        0,
        QualitySettings.names.Length - 1
    );

        tempVsync = PlayerPrefs.GetInt("vSync", 1) == 1;

        QualitySettings.SetQualityLevel(tempQuality);
        QualitySettings.vSyncCount = tempVsync ? 1 : 0;

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
            totalTimer += Time.deltaTime;
            waveTimer += Time.deltaTime;
        }

        if(Input.GetButtonDown("Cancel") && !inGame && menuActive == menuCredits)
        {
            animator.Play("FadoutCredits");
            isFadingOut = true;
        }
        if(isFadingOut)
        {
            var s = animator.GetCurrentAnimatorStateInfo(0);
            if (s.IsName("FadoutCredits") && s.normalizedTime >= 1f)
            {
                endCredits();
                isFadingOut = false;
            }
        }


    }

    public void UpdateTotalScoreText(int amount)
    {
        totalScore += amount;
        totalScoreText.text = totalScore.ToString();
    }
    public void UpdateWaveScoreText(int amount)
    {
        waveScore += amount;
        waveScoreText.text = waveScore.ToString();
    }

    public void UpdateGameGoal(int amount)
    {
        goalCount += amount;
        UpdateWaveEnemyText();
        if(goalCount <= 0)
        {
            Win();
        }
    }

    void UpdateTotalTimerText()
    {
        int totalSeconds = Mathf.FloorToInt(totalTimer);
        int minutes = totalSeconds / 60;
        int seconds = totalSeconds % 60;

        totalTimerText.text = $"{minutes}:{seconds:00}";
    }

    void UpdateWaveTimerText()
    {
        int waveSeconds = Mathf.FloorToInt(waveTimer);
        int minutes = waveSeconds / 60;
        int seconds = waveSeconds % 60;


        waveTimerText.text = $"{minutes}:{seconds:00}";
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
        menuActive = menuCredits;
        menuCredits.SetActive(true);
        animator.Play("Credits");
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

    public void Win()
    {
        statePause();
        UpdateTotalTimerText();
        UpdateWaveTimerText();

        menuActive = menuWin;
        menuActive.SetActive(true);
    }

    public void SetSens(int s)
    {
        sens = s;
        PlayerPrefs.SetInt("sens", s);
        PlayerPrefs.Save();
    }

    public void UpdateWaveEnemyText()
    {
        waveText.text = "WAVE: " + WaveManager.instance.waveNum;
        enemyText.text = "Enemies: " + goalCount;
    }

    public void ResetWaveScore()
    {
        waveScore = 0;
    }
    public void ResetWaveTimer()
    {
        waveTimer = 0;
    }

    public int GetGameGoalCount()
    {
        return goalCount;
    }
}
