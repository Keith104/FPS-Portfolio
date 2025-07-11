using System.Collections.Generic;
using UnityEngine;

public class WaveManager : MonoBehaviour
{
    public static WaveManager instance;

    [Header("Wave Settings")]
    [SerializeField] int baseMinEnemies;
    [SerializeField] int baseMaxEnemies;
    [SerializeField] float enemiesPerWaveIncrease;

    [Header("Enemy Settings")]
    [SerializeField] GameObject[] enemySpawns;
    [SerializeField] GameObject[] enemyPrefabs;
    [SerializeField] LayerMask enemyLayerMask;
    [SerializeField] float spawnCheckRadius;

    [Header("Current Wave")]
    public int waveNum;

    int waveTillMiniBoss;
    int waveTillBoss;

    float minThisWave;
    float maxThisWave;

    Difficulty difficulty;

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

        StartWave();
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

            GameManager.instance.UpdateGameGoal(1);

            Debug.Log(waveNum);
            Debug.Log("BOSS");
            return;
        }else if(waveTillMiniBoss <= -5)
        {
            waveTillMiniBoss = 5;

            GameManager.instance.UpdateGameGoal(1);

            Debug.Log("MINI BOSS!");
            return;
        }

        minThisWave = baseMinEnemies + (waveNum - 1) * enemiesPerWaveIncrease;
        maxThisWave = baseMaxEnemies + (waveNum - 1) * enemiesPerWaveIncrease;

        int amountToSpawn = Mathf.RoundToInt(Random.Range(minThisWave, maxThisWave));

        List<Transform> freeSpawns = new List<Transform>();
        foreach (var go in enemySpawns)
        {
            if (!Physics.CheckSphere(go.transform.position, spawnCheckRadius, enemyLayerMask))
            {
                freeSpawns.Add(go.transform);
            }
        }




        GameManager.instance.UpdateGameGoal(amountToSpawn);
        int spawnCount = Mathf.Min(amountToSpawn, freeSpawns.Count);
        for (int i = 0; i < amountToSpawn; i++)
        {
            int index = Random.Range(0, freeSpawns.Count);
            Transform spawnPoint = freeSpawns[index];

            Instantiate(enemyPrefabs[Random.Range(0, enemyPrefabs.Length)], spawnPoint.position, spawnPoint.rotation);
        }
    }


}
