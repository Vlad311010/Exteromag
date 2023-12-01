using Interfaces;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DamageZone : MonoBehaviour
{
    public int ticks;
    public float tickTime;
    public int damagePerTick;
    public List<IHealthSystem> objectsInDamageZone = new List<IHealthSystem>();


    void Start()
    {
        StartCoroutine(DealDamage());
    }

    IEnumerator DealDamage()
    {
        for (int i = 0; i < ticks; i++)
        {
            yield return new WaitForSeconds(tickTime);
            foreach (IHealthSystem hs in objectsInDamageZone)
            {
                hs.ConsumeHp(damagePerTick, Vector2.zero);
            }
        }
        // destroy obj
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out IHealthSystem hs))
        {
            objectsInDamageZone.Add(hs);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out IHealthSystem hs))
        {
            objectsInDamageZone.Remove(hs);
        }
    }
}
