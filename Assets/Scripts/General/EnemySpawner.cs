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
    [SerializeField] List<WaveSpawnData> waves = new List<WaveSpawnData>();
    [SerializeField] int enemiesCountToTriggerWaveSpawn;

    private int waveIdx = 0;

    private void Start()
    {
        // GameEvents.current.onEnemiesCountChange += TriggerSpawn;
        GameEvents.current.onEnemyDeath += TriggerSpawn;
    }

    private void TriggerSpawn()
    {
        Debug.Log(SceneController.enemiesCount);
        if (SceneController.enemiesCount <= enemiesCountToTriggerWaveSpawn)
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
                Instantiate(enemySpawnData.enemy, RandomSpawnPoint(), Quaternion.identity, transform);
            }
        }
        waveIdx++;
    }

    private Vector2 RandomSpawnPoint()
    {
        Vector2 offset = new Vector2(Random.Range(-size.x, size.x), Random.Range(-size.y, size.y));
        return (Vector2)transform.position + offset;  
    }

    private void OnDrawGizmos()
    {
        Color color = Color.green;
        color.a = 0.2f;
        Gizmos.color = color;

        Gizmos.DrawCube(transform.position, size);
    }
}
