using UnityEngine;

[CreateAssetMenu(fileName = "DifficultyLevel", menuName = "Scriptable Objects/Difficulty Level")]
public class Difficulty : ScriptableObject
{
    [Header("Difficulty Settings")]
    public string difficultyName;

    [Header("Wave Settings")]
    public int baseMinSpawn;
    public int baseMaxSpawn;
    public int maxEnemiesAllowed;
    public float enemiesPerWaveIncrease;

    [Header("Player Settings")]
    public int maxRespawns;
    public bool realisticMode;

}
