using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonFunctions : MonoBehaviour
{
    
    public void Resume()
    {
        GameManager.instance.stateUnPause();
    }

    public void Continue()
    {
        GameManager.instance.stateUnPause();
        WaveManager.instance.StartWave();
    }

    public void Respawn()
    {
        GameManager.instance.playerController.RespawnPlayer();
        GameManager.instance.stateUnPause();
    }

    public void Quit()
    {
#if !UNITY_EDITOR
        Application.Quit();
#else
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }

    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        GameManager.instance.stateUnPause();
    }

    public void LoadLevel(int lvl)
    {
        // When player has ability that needs different time scale set here.
        Time.timeScale = 1f;
        SceneManager.LoadScene(lvl);
    }

    public void SetActiveMenu(GameObject menuActive)
    {
        GameManager.instance.SetActiveMenu(menuActive);
    }

    // Settings Menu
    public void ApplySettings()
    {
        SettingsManager.instance.ApplySettings();
    }

    public void Back()
    {
        SettingsManager.instance.RevertSettings();
    }

    public void backToPauseMenu()
    {
        GameManager.instance.backToPauseMenu();
    }

    public void SetDifficulty(Difficulty difficulty)
    {
        DifficultyManager.instance.SetDifficulty(difficulty);
    }
}
