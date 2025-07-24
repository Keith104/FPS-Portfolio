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
        int count = spawnObjects.Length;
        regEnemySpawns = new Transform[count];
        nonMoveEnemySpawns = new Transform[count];
        for (int i = 0; i < count; i++)
        {
            var t = spawnObjects[i].transform;
            regEnemySpawns[i] = t;
            nonMoveEnemySpawns[i] = t;
        }

        StartWave();
    }

    void Update()
    {
        if (totalToSpawnLeft <= 0) return;

        int aliveEnemies = GameObject.FindGameObjectsWithTag("Enemy").Length;
        if (aliveEnemies < maxEnemiesAllowed)
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
            totalToSpawnLeft = Mathf.RoundToInt(
                Random.Range(baseMinEnemies + increase, baseMaxEnemies + increase)
            );
        }

        int initialSpawn = Mathf.Min(maxEnemiesAllowed, totalToSpawnLeft);
        for (int i = 0; i < initialSpawn; i++)
            SpawnNextEnemy();
    }

    public void EnemyDestroyed()
    {
        if (totalToSpawnLeft <= 0) return;

        int aliveEnemies = GameObject.FindGameObjectsWithTag("Enemy").Length;
        if (aliveEnemies < maxEnemiesAllowed)
            SpawnNextEnemy();
    }

    void SpawnNextEnemy()
    {
        bool regular = Random.value < 0.5f;
        var spawns = regular ? regEnemySpawns : nonMoveEnemySpawns;
        var prefabs = regular ? regEnemyPrefabs : nonMoveEnemyPrefabs;

        freeSpawns.Clear();
        foreach (var t in spawns)
            if (!Physics.CheckSphere(t.position, spawnCheckRadius, enemyLayerMask))
                freeSpawns.Add(t);

        if (freeSpawns.Count == 0) return;

        var spawnPoint = freeSpawns[Random.Range(0, freeSpawns.Count)];
        Instantiate(prefabs[Random.Range(0, prefabs.Length)], spawnPoint.position, spawnPoint.rotation);

        totalToSpawnLeft--;
        GameManager.instance.UpdateGameGoal(totalToSpawnLeft);
    }
}
