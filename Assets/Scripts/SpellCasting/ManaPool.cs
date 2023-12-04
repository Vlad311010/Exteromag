using UnityEngine;

public class ManaPool : MonoBehaviour
{
    [SerializeField] private int mpRestorePerKill;
    [SerializeField] private int maxMp;
    private int currentMp;


    private void Start()
    {
        currentMp = maxMp;
        Consume(0);
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

    private void OnDestroy()
    {
        GameEvents.current.onEnemyDeath -= GetMpFromEnemy;
    }
}
