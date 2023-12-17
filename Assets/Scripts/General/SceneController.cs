using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
    [SerializeField] Vector2 exitPos;
    [SerializeField] GameObject exit;

    // [SerializeField] UpgradeWindowUI spellUpgradeMenu;
    static CharacterLimitations characterLimitations;

    public int enemiesCount { get; private set; }
    public int spawnersCount { get; private set; }

    void Start()
    {
        GameEvents.current.onSceneLoad += LoadScene;
        GameEvents.current.onEnemyDeath += OnEnemyDeath;
        GameEvents.current.onEnemySpawn += OnEnemySpawn;
        GameEvents.current.onSpawnerDestroy += OnSpawnerDestroy;
        
        // GameEvents.current.onEscapePressed += OnEscapePressed;
        // GameEvents.current.onUpgradePickUp += OpenSpellUpgradeMenu;


        enemiesCount = GameObject.FindGameObjectsWithTag("Enemy").Length;
        spawnersCount = GameObject.FindObjectsOfType<EnemySpawner>().Length;
        characterLimitations = GameObject.FindGameObjectWithTag("Player")?.GetComponent<CharacterLimitations>();
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

    /*private void OnEscapePressed()
    {
        GameObject activeWindow = GameObject.FindGameObjectWithTag("WindowUI");
        if (activeWindow == null)
        {
            Pause();
            OpenMenu();
        }
        else
        {
            CloseActiveWindow();
        }
    }*/

    /*private void OpenMenu()
    {
        Pause();
    }*/

    /*private void OpenSpellUpgradeMenu()
    {
        Pause();
        spellUpgradeMenu.gameObject.SetActive(true);
        spellUpgradeMenu.Activate();
    }*/

    /*private void CloseActiveWindow()
    {
        // GameObject.FindGameObjectWithTag("WindowUI");
        Resume();
    }*/

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
