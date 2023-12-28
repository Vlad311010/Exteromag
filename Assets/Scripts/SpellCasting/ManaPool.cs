using Interfaces;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ManaPool : MonoBehaviour, IResatable
{
    [SerializeField] private int mpRestorePerKill;
    [SerializeField] private int maxMp;
    private int currentMp;

    public int MaxMp { get => maxMp; }
    public int CurrentMp { get => currentMp; }

    private void Start()
    {
        currentMp = maxMp;
        Consume(0);
        SceneManager.sceneLoaded += UpdateUI;
        GameEvents.current.onEnemyDeath += GetMpFromEnemy;
    }

    public bool HaveEnaughtMp(int toConsume)
    {
        return currentMp >= toConsume;
    }


    public void Consume(int amount)
    {
        // StopAllCoroutines();
        currentMp = Mathf.Clamp(currentMp - amount, 0, maxMp);
        GameEvents.current.ManaChange(currentMp, maxMp);
        // StartCoroutine(Restore());
    }

    public void GetMpFromEnemy()
    {
        Consume(-mpRestorePerKill);
    }

    public void ResetValues()
    {
        currentMp = SceneController.characterStats.mana;
        GameEvents.current.ManaChange(currentMp, maxMp);
    }

    private void UpdateUI(Scene scene, LoadSceneMode mode)
    {
        GameEvents.current.ManaChange(currentMp, maxMp);
    }

    private void OnDestroy()
    {
        GameEvents.current.onEnemyDeath -= GetMpFromEnemy;
        SceneManager.sceneLoaded -= UpdateUI;
    }
}
