using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public struct EnemySpawnData
{
    public GameObject enemy;
    public int count;
}

[System.Serializable]
public struct WaveSpawnData
{
    public List<EnemySpawnData> enemies;
}

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] Vector2 size;


    [SerializeField] bool setGoToZone = false;
    [SerializeField] Vector2 goToZonePos;
    [SerializeField] Vector2 goToZoneSize;

    [SerializeField] List<WaveSpawnData> waves = new List<WaveSpawnData>();
    [SerializeField] int enemiesCountToTriggerWaveSpawn;

    private int waveIdx = 0;

    private void OnEnable()
    {
        // GameEvents.current.onEnemyDeath += TriggerSpawn;
        GameEvents.current.onEnemiesCountChange += TriggerSpawn;
    }

    private void OnDisable()
    {
        // GameEvents.current.onEnemyDeath -= TriggerSpawn;
        GameEvents.current.onEnemiesCountChange -= TriggerSpawn;
    }


    private void TriggerSpawn(int enemiesCount)
    {
        if (enemiesCount <= enemiesCountToTriggerWaveSpawn)
        {
            SpawnWave();
        }
    }


    private void SpawnWave()
    {
        foreach (EnemySpawnData enemySpawnData in waves[waveIdx].enemies)
        {
            for (int i = 0; i < enemySpawnData.count; i++)
            {
                AICore aiCore = Instantiate(enemySpawnData.enemy, RandomSpawnPoint(transform.position, size), Quaternion.identity, transform).GetComponent<AICore>();
                if (setGoToZone)
                {
                    aiCore.SetGoToPoint(RandomSpawnPoint(goToZonePos, goToZoneSize));
                }
                GameEvents.current.EnemySpawned();
            }
        }
        waveIdx++;
        if (waveIdx >= waves.Count)
        {
            GameEvents.current.SpawnerDestroy();
            Destroy(this);
        }
    }

    private Vector2 RandomSpawnPoint(Vector2 zonePos, Vector2 zoneSize)
    {
        Vector2 offset = new Vector2(Random.Range(-zoneSize.x/2, zoneSize.x/2), Random.Range(-zoneSize.y/2, zoneSize.y/2));
        return zonePos+ offset;  
    }

    private void OnDrawGizmos()
    {
        Extensions.GizmosSetColor(UnityEngine.Color.green, 0.2f);
        Gizmos.DrawCube(transform.position, size);

        Gizmos.DrawLine(transform.position, goToZonePos);

        Extensions.GizmosSetColor(UnityEngine.Color.yellow, 0.4f);
        Gizmos.DrawCube(goToZonePos, goToZoneSize);
    }
}
