using UnityEngine;

[CreateAssetMenu(fileName = "DifficultyLevel", menuName = "Scriptable Objects/Difficulty Level")]
public class Difficulty : ScriptableObject
{
    public string difficultyName;
    public int baseMinSpawn;
    public int baseMaxSpawn;
    public float enemiesPerWaveIncrease;

}
