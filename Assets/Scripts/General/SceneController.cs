using Interfaces;
using NavMeshPlus.Components;
using System.Collections;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
    [SerializeField] GameObject playerPrefab;
    [SerializeField] Vector2 spawPos;

    [SerializeField] Vector2 exitPos;
    [SerializeField] GameObject exit;


    public static CharacterStatsSO characterStats;
    public static SettingsSO gameSettings;
    private static CharacterLimitations characterLimitations;
    private static NavMeshSurface navMesh;
    private static Coroutine navMeshBakingCoroutine = null;
    private static SceneController self;
    public int enemiesCount { get; private set; }
    public int spawnersCount { get; private set; }

    private void Awake()
    {
        SpawnPlayer();
        characterStats = characterStats ?? ScriptableObject.CreateInstance("CharacterStatsSO") as CharacterStatsSO;
        gameSettings = ScriptableObject.CreateInstance("SettingsSO") as SettingsSO;
    }


    void Start()
    {
        self = this;

        GameEvents.current.onEnemyDeath += OnEnemyDeath;
        GameEvents.current.onEnemySpawn += OnEnemySpawn;
        GameEvents.current.onSpawnerDestroy += OnSpawnerDestroy;
        GameEvents.current.onExitTriggered += SavePlayerStats;

        
        enemiesCount = GameObject.FindGameObjectsWithTag("Enemy").Length;
        spawnersCount = GameObject.FindObjectsOfType<EnemySpawner>().Length;
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        characterLimitations = player.GetComponent<CharacterLimitations>();

        navMesh = GameObject.FindGameObjectWithTag("NavMesh")?.GetComponent<NavMeshSurface>();
        
        
    }

    private void SavePlayerStats()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        characterStats.hp = player.GetComponent<IHealthSystem>().CurrentHealth;
        characterStats.mana = player.GetComponent<ManaPool>().CurrentMp;
        Debug.Log(characterStats.hp + " " + characterStats.mana);
    }

    private void OnDestroy()
    {
        GameEvents.current.onEnemyDeath -= OnEnemyDeath;
        GameEvents.current.onSpawnerDestroy -= OnSpawnerDestroy;
    }

    private void SpawnPlayer()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player == null)
        {
            Instantiate(playerPrefab, spawPos, Quaternion.identity);
        }
        else
        {
            player.transform.position = spawPos;
        }
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

    private static void LoadScene(int sceneIdx)
    {
        SceneManager.LoadScene(sceneIdx);
    }

    public static void BakeNavMesh(bool immediate = false)
    {
        if (immediate)
        {
            navMesh.BuildNavMeshAsync();
            return;
        }

        if (navMeshBakingCoroutine != null)
            self.StopCoroutine(navMeshBakingCoroutine);

        navMeshBakingCoroutine = self.StartCoroutine(NavMeshBakingDelay());
    }

    static IEnumerator NavMeshBakingDelay()
    {
        yield return new WaitForSecondsRealtime(0.5f);
        navMesh.BuildNavMeshAsync();
    }

    public static void ReloadScene()
    {
        GameObject.FindGameObjectWithTag("Player").GetComponent<CharacterRespawn>().Respawn();
        LoadScene(SceneManager.GetActiveScene().buildIndex);
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

    private void OnDrawGizmos()
    {
        #if UNITY_EDITOR
            if (EditorApplication.isPlaying) return;
        #endif

        Gizmos.color = Color.green;
        Gizmos.DrawCube(spawPos, Vector2.one);

        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(exitPos, Vector2.one);
    }
}
