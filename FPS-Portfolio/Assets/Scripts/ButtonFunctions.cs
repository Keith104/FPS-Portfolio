using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonFunctions : MonoBehaviour
{

    public void Resume()
    {
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

    public void ToMainMenu()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene("MainMenu");
    }

    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        GameManager.instance.stateUnPause();
    }

    public void LoadLevel(int lvl)
    {
        // When player has ability that needs different time scale set here.

        SceneManager.LoadScene(lvl);
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
}
