using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonFunctions : MonoBehaviour
{

    public void Play()
    {
        SceneManager.LoadScene("GameScene");
    }


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
        SceneManager.LoadScene("MainMenu");
    }
}
