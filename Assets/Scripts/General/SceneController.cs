using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
    public static int enemiesCount { get; private set; }
    public static int spawnersCount { get; private set; }

    void Start()
    {
        GameEvents.current.onSceneLoad += LoadScene;
        GameEvents.current.onEnemyDeath += OnEnemyDeath;

        enemiesCount = GameObject.FindGameObjectsWithTag("Enemy").Length;
        spawnersCount = GameObject.FindObjectsOfType<EnemySpawner>().Length;
    }

    private void OnEnemyDeath()
    {
        enemiesCount = GameObject.FindGameObjectsWithTag("Enemy").Length;
        Debug.Log("SCENE COnt");
        // GameEvents.current.EnemiesCountChange(enemiesCount);
        /*if (enemiesCount == 0)
        {

            // else load next level
        }*/
    }

    private void LoadScene(int sceneIdx)
    {
        SceneManager.LoadScene(sceneIdx);
    }
}
