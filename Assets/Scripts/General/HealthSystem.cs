using Interfaces;
using System;
using UnityEngine;

public class HealthSystem : MonoBehaviour
{
    [SerializeField] int maxHp;
    
    private int currentHp;

    void Start()
    {
        currentHp = maxHp;
    }

    public void ConsumeHp(int amount)
    {
        currentHp = Math.Clamp(currentHp - amount, 0, maxHp);
        GameEvents.current.HealthChange(currentHp, maxHp);
        if (currentHp <= 0)
            GetComponent<IDestroyable>().DestroyObject();
    }


}
