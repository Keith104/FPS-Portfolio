using UnityEngine;

public class WaveManager : MonoBehaviour
{
    public static WaveManager instance;

    [SerializeField] int baseMinEnemies;
    [SerializeField] int baseMaxEnemies;
    [SerializeField] int enemiesPerWaveIncrease;
    [SerializeField] GameObject[] enemySpawns;
    [SerializeField] GameObject[] enemyPrefabs;
    [SerializeField] LayerMask enemyLayerMask;
    [SerializeField] float spawnCheckRadius;

    public int waveNum;


    int minThisWave;
    int maxThisWave;

    private void Start()
    {
        instance = this;

        enemySpawns = GameObject.FindGameObjectsWithTag("EnemySpawn");

        StartWave();
    }

    public void StartWave()
    {
        waveNum++;

        minThisWave = baseMinEnemies + (waveNum - 1) * enemiesPerWaveIncrease;
        maxThisWave = baseMaxEnemies + (waveNum - 1) * enemiesPerWaveIncrease;

        int amountToSpawn = Random.Range(minThisWave, maxThisWave);
        GameManager.instance.UpdateGameGoal(amountToSpawn);
        for(int i = 0; i < amountToSpawn; i++)
        {
            Transform spawnPoint = enemySpawns[Random.Range(0, enemySpawns.Length)].transform;

            if (Physics.CheckSphere(spawnPoint.position, spawnCheckRadius, enemyLayerMask))
            {
                i--;
                continue;
            }
            Instantiate(enemyPrefabs[Random.Range(0, enemyPrefabs.Length)], spawnPoint.position, spawnPoint.rotation);
        }
    }
}
