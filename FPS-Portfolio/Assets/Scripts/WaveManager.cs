using System.Collections.Generic;
using UnityEngine;

public class WaveManager : MonoBehaviour
{
    public static WaveManager instance;

    [Header("Wave Settings")]
    [SerializeField] int baseMinEnemies;
    [SerializeField] int baseMaxEnemies;
    [SerializeField] float enemiesPerWaveIncrease;
    [SerializeField] int maxEnemiesAllowed;

    [Header("Enemy Settings")]
    [SerializeField] GameObject[] enemySpawns;
    [SerializeField] GameObject[] enemyPrefabs;
    [SerializeField] LayerMask enemyLayerMask;
    [SerializeField] float spawnCheckRadius;

    [Header("Current Wave")]
    public int waveNum;

    int waveTillMiniBoss;
    int waveTillBoss;
    Difficulty difficulty;
    int totalToSpawnLeft;

    private void Start()
    {
        instance = this;
        enemySpawns = GameObject.FindGameObjectsWithTag("EnemySpawn");
        waveTillMiniBoss = 5;
        waveTillBoss = 10;
        difficulty = DifficultyManager.instance.GetDifficulty();
        baseMinEnemies = difficulty.baseMinSpawn;
        baseMaxEnemies = difficulty.baseMaxSpawn;
        enemiesPerWaveIncrease = difficulty.enemiesPerWaveIncrease;
        maxEnemiesAllowed = difficulty.maxEnemiesAllowed;
        StartWave();
    }

    private void Update()
    {
        if (totalToSpawnLeft <= 0) return;
        int current = GameManager.instance.GetGameGoalCount();
        int spawnCount = Mathf.Min(maxEnemiesAllowed - current, totalToSpawnLeft);
        for (int i = 0; i < spawnCount; i++)
        {
            SpawnEnemy();
            totalToSpawnLeft--;
        }
    }

    public void StartWave()
    {
        totalToSpawnLeft = 0;
        waveTillMiniBoss--;
        waveTillBoss--;
        waveNum++;
        GameManager.instance.ResetWaveScore();
        GameManager.instance.ResetWaveTimer();

        if (waveTillBoss <= 0)
        {
            waveTillBoss = 10;
            waveTillMiniBoss = 5;
            totalToSpawnLeft = 1;
            GameManager.instance.UpdateGameGoal(1);
        }
        else if (waveTillMiniBoss <= 0)
        {
            waveTillMiniBoss = 5;
            totalToSpawnLeft = 1;
            GameManager.instance.UpdateGameGoal(1);
        }
        else
        {
            float minThisWave = baseMinEnemies + (waveNum - 1) * enemiesPerWaveIncrease;
            float maxThisWave = baseMaxEnemies + (waveNum - 1) * enemiesPerWaveIncrease;
            int amountToSpawn = Mathf.RoundToInt(Random.Range(minThisWave, maxThisWave));
            totalToSpawnLeft = amountToSpawn;
            GameManager.instance.UpdateGameGoal(amountToSpawn);
            int initialSpawn = Mathf.Min(maxEnemiesAllowed, totalToSpawnLeft);
            for (int i = 0; i < initialSpawn; i++)
            {
                SpawnEnemy();
                totalToSpawnLeft--;
            }
        }
    }

    void SpawnEnemy()
    {
        List<Transform> freeSpawns = new List<Transform>();
        foreach (var go in enemySpawns)
            if (!Physics.CheckSphere(go.transform.position, spawnCheckRadius, enemyLayerMask))
                freeSpawns.Add(go.transform);
        if (freeSpawns.Count == 0) return;
        Transform spawnPoint = freeSpawns[Random.Range(0, freeSpawns.Count)];
        Instantiate(enemyPrefabs[Random.Range(0, enemyPrefabs.Length)], spawnPoint.position, spawnPoint.rotation);
    }
}
