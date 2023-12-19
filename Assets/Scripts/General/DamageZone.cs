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

    [SerializeField] LayerMask layerMask;

    private List<Transform> objectsInDamageZone = new List<Transform>();


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
            foreach (Transform o in objectsInDamageZone)
            {
                Vector2 outOfCenterDirection = (o.position - transform.position).normalized;
                outOfCenterDirection = outOfCenterDirection == Vector2.zero ? Vector2.up : outOfCenterDirection;
                o.GetComponent<IHealthSystem>().ConsumeHp(damagePerTick, outOfCenterDirection);
            }
        }
        Deactivate();
    }

    

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (layerMask.CheckLayer(collision.gameObject.layer) && collision.TryGetComponent(out IHealthSystem hs))
        {
            objectsInDamageZone.Add(collision.transform);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (layerMask.CheckLayer(collision.gameObject.layer) && collision.TryGetComponent(out IHealthSystem hs))
        {
            objectsInDamageZone.Remove(collision.transform);
        }
    }
}
