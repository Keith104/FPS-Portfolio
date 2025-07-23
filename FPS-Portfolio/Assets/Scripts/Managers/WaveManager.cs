using UnityEngine;
using System.Collections.Generic;

public class WaveManager : MonoBehaviour
{
    public static WaveManager instance;

    [Header("Wave Settings")]
    [SerializeField] int baseMinEnemies, baseMaxEnemies, maxEnemiesAllowed;
    [SerializeField] float enemiesPerWaveIncrease;
    [SerializeField] Difficulty difficulty;

    [Header("Enemy Settings")]
    [SerializeField] Transform[] regEnemySpawns;
    [SerializeField] GameObject[] regEnemyPrefabs;
    [SerializeField] Transform[] nonMoveEnemySpawns;
    [SerializeField] GameObject[] nonMoveEnemyPrefabs;
    [SerializeField] LayerMask enemyLayerMask;
    [SerializeField] float spawnCheckRadius;

    [Header("Current Wave")]
    public int waveNum;

    int waveTillMiniBoss = 5;
    int waveTillBoss = 10;
    int totalToSpawnLeft;
    List<Transform> freeSpawns = new List<Transform>();

    void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(gameObject);
    }

    void Start()
    {
        difficulty = DifficultyManager.instance.GetDifficulty();
        baseMinEnemies = difficulty.baseMinSpawn;
        baseMaxEnemies = difficulty.baseMaxSpawn;
        enemiesPerWaveIncrease = difficulty.enemiesPerWaveIncrease;
        maxEnemiesAllowed = difficulty.maxEnemiesAllowed;

        var spawnObjects = GameObject.FindGameObjectsWithTag("EnemySpawn");
        regEnemySpawns = new Transform[spawnObjects.Length];
        for (int i = 0; i < spawnObjects.Length; i++)
            regEnemySpawns[i] = spawnObjects[i].transform;

        StartWave();
    }

    void Update()
    {
        if (totalToSpawnLeft <= 0) return;
        int currentCount = GameManager.instance.GetGameGoalCount();
        int availableSlots = maxEnemiesAllowed - currentCount;
        if (availableSlots <= 0) return;
        int spawnCount = Mathf.Min(availableSlots, totalToSpawnLeft);
        for (int i = 0; i < spawnCount; i++)
            SpawnNextEnemy();
    }

    public void StartWave()
    {
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
        }
        else if (waveTillMiniBoss <= 0)
        {
            waveTillMiniBoss = 5;
            totalToSpawnLeft = 1;
        }
        else
        {
            float increase = (waveNum - 1) * enemiesPerWaveIncrease;
            int amount = Mathf.RoundToInt(
                Random.Range(baseMinEnemies + increase, baseMaxEnemies + increase)
            );
            totalToSpawnLeft = amount;
        }

        int initialSpawn = Mathf.Min(maxEnemiesAllowed, totalToSpawnLeft);
        for (int i = 0; i < initialSpawn; i++)
            SpawnNextEnemy();
    }

    void SpawnNextEnemy()
    {
        bool spawnRegular = Random.value < 0.5f;
        var spawns = spawnRegular ? regEnemySpawns : nonMoveEnemySpawns;
        var prefabs = spawnRegular ? regEnemyPrefabs : nonMoveEnemyPrefabs;

        freeSpawns.Clear();
        foreach (var t in spawns)
            if (!Physics.CheckSphere(t.position, spawnCheckRadius, enemyLayerMask))
                freeSpawns.Add(t);

        if (freeSpawns.Count == 0) return;

        var spawnPoint = freeSpawns[Random.Range(0, freeSpawns.Count)];
        Instantiate(
            prefabs[Random.Range(0, prefabs.Length)],
            spawnPoint.position,
            spawnPoint.rotation
        );
        GameManager.instance.UpdateGameGoal(totalToSpawnLeft);
        totalToSpawnLeft--;
    }
}
