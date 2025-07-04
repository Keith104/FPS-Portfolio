using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonFunctions : MonoBehaviour
{

    public void Play()
    {
        SceneManager.LoadScene(1);
    }


    public void Resume()
    {
        Gamemanager.instance.stateUnPause();
    }

    public void Quit()
    {
#if !UNITY_EDITOR
        Application.Quit();
#else
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
}
