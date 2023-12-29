using System;
using UnityEngine;

public class GameEvents : MonoBehaviour
{
    public static GameEvents current;
    private void Awake()
    {
        current = this;
    }
    public event Action<int, int> onHealthChange;
    public void HealthChange(int current, int max)
    {
        if (onHealthChange != null)
        {
            onHealthChange(current, max);
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

    public event Action<int, int> onHealthChangePlayer;
    public void HealthChangePlayer(int current, int max)
    {
        if (onHealthChangePlayer != null)
        {
            onHealthChangePlayer(current, max);
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

    public event Action onEnemySpawn;
    public void EnemySpawned()
    {
        if (onEnemySpawn != null)
        {
            onEnemySpawn();
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

    public event Action onPlayerSpawn;
    public void PlayerSpawned()
    {
        if (onPlayerSpawn != null)
        {
            onPlayerSpawn();
        }
    }

    public event Action onRespawn;
    public void Respawn()
    {
        if (onRespawn != null)
        {
            onRespawn();
        }
    }

    public event Action onNextLevelLoad;
    public void NextLevelLoad()
    {
        if (onNextLevelLoad != null)
        {
            onNextLevelLoad();
        }
    }

    public event Action onExitTriggered;
    public void ExitTriggered()
    {
        if (onExitTriggered != null)
        {
            onExitTriggered();
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

    public event Action onSpawnerDestroy;
    public void SpawnerDestroy()
    {
        if (onSpawnerDestroy != null)
        {
            onSpawnerDestroy();
        }
    }

    public event Action onEscapePressed;
    public void EscapePressed()
    {
        if (onEscapePressed != null)
        {
            onEscapePressed();
        }
    }

    public event Action onUpgradePickUp;
    public void UpgradePickUp()
    {
        if (onUpgradePickUp != null)
        {
            onUpgradePickUp();
        }
    }

    public event Action<int> onBlockerActivation;
    public void BlockerActivation(int id)
    {
        if (onBlockerActivation != null)
        {
            onBlockerActivation(id);
        }
    }

}
