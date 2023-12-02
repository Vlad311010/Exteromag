using Interfaces;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageZone : MonoBehaviour
{
    [SerializeField] Material activeMaterial;
    [SerializeField] Material unactiveMaterial;

    [SerializeField] float activationTime;
    [SerializeField] float tickTime;
    [SerializeField] int ticks;
    [SerializeField] int damagePerTick;
    
    private List<IHealthSystem> objectsInDamageZone = new List<IHealthSystem>();


    void Start()
    {
        GetComponent<SpriteRenderer>().material = unactiveMaterial;
        StartCoroutine(ActivationTimer());
    }

    IEnumerator ActivationTimer()
    {
        yield return new WaitForSeconds(activationTime);
        GetComponent<SpriteRenderer>().material = activeMaterial;
        StartCoroutine(DealDamage());
    }

    private void Deactivate()
    {
        Destroy(this.gameObject);
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
        Deactivate();
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
