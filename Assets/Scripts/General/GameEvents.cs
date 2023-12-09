using System;
using UnityEngine;

public class GameEvents : MonoBehaviour
{
    public static GameEvents current;
    private void Awake()
    {
        current = this;
    }

    public event Action<int> onSceneLoad;
    public void SceneLoad(int sceneIdx)
    {
        if (onSceneLoad != null)
        {
            onSceneLoad(sceneIdx);
        }
    }

    public event Action<int, int> onManaChange;
    public void ManaChange(int current, int max)
    {
        if (onManaChange != null)
        {
            onManaChange(current, max);
        }
    }

    public event Action<int, int> onHealthChange;
    public void HealthChange(int current, int max)
    {
        if (onHealthChange != null)
        {
            onHealthChange(current, max);
        }
    }

    public event Action onEnemyDeath;
    public void EnemyDied()
    {
        if (onEnemyDeath != null)
        {
            onEnemyDeath();
        }
    }

    public event Action<float, int> onSpellCooldownValueChange;
    public void SpellCooldownValueChange(float percent, int slotIdx)
    {
        if (onSpellCooldownValueChange != null)
        {
            onSpellCooldownValueChange(percent, slotIdx);
        }
    }

    public event Action<bool, int> onSpellCastButtonHold;
    public void SpellCastButtonHold(bool holding, int slotIdx)
    {
        if (onSpellCastButtonHold != null)
        {
            onSpellCastButtonHold(holding, slotIdx);
        }
    }

    public event Action<Protect> onProtectorsDeath;
    public void ProtectorDied(Protect protector)
    {
        if (onProtectorsDeath != null)
        {
            onProtectorsDeath(protector);
        }
    }

    public event Action onPlayersDeath;
    public void PlayerDied()
    {
        if (onPlayersDeath!= null)
        {
            onPlayersDeath();
        }
    }

    public event Action<int> onEnemiesCountChange;
    public void EnemiesCountChange(int enemiesCount)
    {
        if (onEnemiesCountChange != null)
        {
            onEnemiesCountChange(enemiesCount);
        }
    }
}
