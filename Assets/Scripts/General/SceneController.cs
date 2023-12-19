using NavMeshPlus.Components;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
    [SerializeField] Vector2 exitPos;
    [SerializeField] GameObject exit;

    static private CharacterLimitations characterLimitations;
    static private NavMeshSurface navMesh;
    static private Coroutine navMeshBakingCoroutine = null;
    static private SceneController self;

    public int enemiesCount { get; private set; }
    public int spawnersCount { get; private set; }


    void Start()
    {
        self = this;
        BakeNavMesh();

        GameEvents.current.onSceneLoad += LoadScene;
        GameEvents.current.onEnemyDeath += OnEnemyDeath;
        GameEvents.current.onEnemySpawn += OnEnemySpawn;
        GameEvents.current.onSpawnerDestroy += OnSpawnerDestroy;
        
        enemiesCount = GameObject.FindGameObjectsWithTag("Enemy").Length;
        spawnersCount = GameObject.FindObjectsOfType<EnemySpawner>().Length;
        characterLimitations = GameObject.FindGameObjectWithTag("Player")?.GetComponent<CharacterLimitations>();
        navMesh = GameObject.FindGameObjectWithTag("NavMesh")?.GetComponent<NavMeshSurface>();
    }

    private void OnDestroy()
    {
        GameEvents.current.onSceneLoad -= LoadScene;
        GameEvents.current.onEnemyDeath -= OnEnemyDeath;
        GameEvents.current.onSpawnerDestroy -= OnSpawnerDestroy;
    }

    private void OnEnemyDeath()
    {
        enemiesCount--;
        GameEvents.current.EnemiesCountChange(enemiesCount);
        if (enemiesCount == 0 && spawnersCount == 0)
        {
            Instantiate(exit, exitPos, Quaternion.identity);
        }
    }
    
    private void OnEnemySpawn()
    {
        enemiesCount++;
    }

    private void OnSpawnerDestroy()
    {
        spawnersCount--;
    }

    private void LoadScene(int sceneIdx)
    {
        SceneManager.LoadScene(sceneIdx);
    }

    public static void BakeNavMesh()
    {
        if (navMeshBakingCoroutine != null)
            self.StopCoroutine(navMeshBakingCoroutine);

        navMeshBakingCoroutine = self.StartCoroutine(NavMeshBakingDelay());
    }

    static IEnumerator NavMeshBakingDelay()
    {
        yield return new WaitForSecondsRealtime(0.5f);
        navMesh.BuildNavMeshAsync();
    }


    public static void Pause()
    {
        Time.timeScale = 0f;
        characterLimitations.DisableActions();
    }

    public static void Resume()
    {
        Time.timeScale = 1f;
        characterLimitations.ActivateActions();
    }
}
