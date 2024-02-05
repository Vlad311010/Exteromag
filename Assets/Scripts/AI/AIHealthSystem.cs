using Interfaces;
using UnityEngine;

public class AIHealthSystem : MonoBehaviour, IHealthSystem
{
    AIEffects effects;
    AICore core;

    public int CurrentHealth { get => currentHp; }

    [SerializeField] int maxHp;
    private int currentHp;
    

    void Start()
    {
        currentHp = maxHp;
        effects = GetComponent<AIEffects>();
        core = GetComponent<AICore>();
    }

    public void ConsumeHp(int amount, Vector2 staggerDirectiom, bool noDamageEffect = false)
    {
        currentHp = System.Math.Clamp(currentHp - amount, 0, maxHp);
        GameEvents.current.HealthChange(currentHp, maxHp);
        if (currentHp <= 0)
        {
            GetComponent<IDestroyable>().DestroyObject();
            this.enabled = false;
        }
        else if (amount > 0)
        {
            effects.OnHit();
        }
    }

}
