using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    public GameObject enemyPrefab;
    public GameObject powerupPrefab;
    private float spawnRangeEnemy = 45;
    private float spawnRangePowerup = 25;
    public int enemyCount;
    public int waveNumber = 1;
    // Start is called before the first frame update
    void Start()
    {
        SpawnEnemyWave(waveNumber);
    }
    void SpawnEnemyWave(int enemiesToSpawn)
    {
        for (int i = 0; i < enemiesToSpawn; i++)
        {
            Instantiate(enemyPrefab, GenerateSpawnPositionEnemy(), enemyPrefab.transform.rotation);
        }
    }

    // Update is called once per frame
    void Update()
    {
        enemyCount = FindObjectsOfType<Enemy>().Length;
        if (enemyCount == 0)
        {
            waveNumber++;
            Instantiate(powerupPrefab, GenerateSpawnPosition(), powerupPrefab.transform.rotation);
            SpawnEnemyWave(waveNumber);
        }
    }

    private Vector3 GenerateSpawnPositionEnemy()
    {
        float spawnPosX = Random.Range(-spawnRangeEnemy, spawnRangeEnemy);
        float spawnPosZ = Random.Range(-spawnRangeEnemy, spawnRangeEnemy);
        Vector3 randomSpawnPos = new Vector3(spawnPosX, 0.7f, spawnPosZ);
        return randomSpawnPos;
    }
    private Vector3 GenerateSpawnPosition()
    {
        float spawnPosX = Random.Range(-spawnRangePowerup, spawnRangePowerup);
        float spawnPosZ = Random.Range(-spawnRangePowerup, spawnRangePowerup);
        Vector3 randomSpawnPos = new Vector3(spawnPosX, 0.7f, spawnPosZ);
        return randomSpawnPos;
    }
}
