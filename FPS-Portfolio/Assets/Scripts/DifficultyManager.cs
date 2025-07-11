using UnityEngine;

public class DifficultyManager : MonoBehaviour
{
    public static DifficultyManager instance;

    [SerializeField] Difficulty difficulty;

    private void Awake()
    {
        instance = this;

        DontDestroyOnLoad(gameObject);
    }

    public void SetDifficulty(Difficulty diff)
    {
        difficulty = diff;
    }

    public Difficulty GetDifficulty()
    {
        return difficulty;
    }
}
