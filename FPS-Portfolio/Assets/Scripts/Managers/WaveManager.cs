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

        regEnemySpawns = GetSpawners("RegEnemySpawn");
        nonMoveEnemySpawns = GetSpawners("NonMoveEnemySpawn");

        StartWave();
    }

    Transform[] GetSpawners(string tag)
    {
        var objs = GameObject.FindGameObjectsWithTag(tag);
        var arr = new Transform[objs.Length];
        for (int i = 0; i < objs.Length; i++)
            arr[i] = objs[i].transform;
        return arr;
    }

    public void StartWave()
    {
        waveNum++;
        waveTillMiniBoss--;
        waveTillBoss--;

        GameManager.instance.ResetWaveScore();
        GameManager.instance.ResetWaveTimer();

        // Determine how many to spawn this wave
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
            // Calculate integer increase per wave
            int inc = Mathf.FloorToInt((waveNum - 1) * enemiesPerWaveIncrease);
            int minCount = baseMinEnemies + inc;
            int maxCount = baseMaxEnemies + inc;
            // Random.Range(int, int) max is exclusive, so add 1
            totalToSpawnLeft = Random.Range(minCount, maxCount + 1);
        }

        // Update UI with total to spawn
        GameManager.instance.UpdateGameGoal(totalToSpawnLeft);

        // Spawn initial batch (up to maxAllowed)
        int initialSpawn = Mathf.Min(totalToSpawnLeft, maxEnemiesAllowed);
        for (int i = 0; i < initialSpawn; i++)
        {
            SpawnEnemy();
            totalToSpawnLeft--;
        }

        // Update UI after initial spawn
        GameManager.instance.UpdateGameGoal(totalToSpawnLeft);
    }

    public void EnemyDestroyed()
    {
        if (totalToSpawnLeft <= 0) return;

        // Only spawn if under the allowed concurrent enemy limit
        int alive = GameObject.FindGameObjectsWithTag("Enemy").Length;
        if (alive < maxEnemiesAllowed)
        {
            SpawnEnemy();
            totalToSpawnLeft--;
            GameManager.instance.UpdateGameGoal(totalToSpawnLeft);
        }
    }

    void SpawnEnemy()
    {
        bool regular = Random.value < 0.5f;
        var spawners = regular ? regEnemySpawns : nonMoveEnemySpawns;
        var prefabs = regular ? regEnemyPrefabs : nonMoveEnemyPrefabs;

        freeSpawns.Clear();
        foreach (var t in spawners)
            if (!Physics.CheckSphere(t.position, spawnCheckRadius, enemyLayerMask))
                freeSpawns.Add(t);

        if (freeSpawns.Count == 0)
            freeSpawns.AddRange(spawners);

        var spawnPoint = freeSpawns[Random.Range(0, freeSpawns.Count)];
        Instantiate(
            prefabs[Random.Range(0, prefabs.Length)],
            spawnPoint.position,
            spawnPoint.rotation
        );
    }
}
