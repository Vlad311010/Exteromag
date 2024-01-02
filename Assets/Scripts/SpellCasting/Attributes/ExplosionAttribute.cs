using UnityEngine;
using Interfaces;
using Structs;

public class ExplosionAttribute : MonoBehaviour, ISpellAttribute
{
    private float radius;
    private float velocityImpact;
    private int damage;
    private LayerMask obstaclelayerMask;
    private LayerMask interactionlayerMask;
    private GameObject explosionEffect;

    public void GetAttributeParameters(SpellScriptableObject spell)
    {
        radius = spell.explosionAttribute.radius;
        velocityImpact = spell.explosionAttribute.velocityImpact;
        damage = spell.explosionAttribute.damage;
        obstaclelayerMask = spell.explosionAttribute.obstacleLayerMask;
        interactionlayerMask = spell.explosionAttribute.interactionLayerMask;
        explosionEffect = spell.explosionAttribute.explosionEffect;
    }

    public void OnCastEvent()
    {
        
    }

    public void OnHitEvent(CollisionData collisionData)
    {
        GameObject effect = Instantiate(explosionEffect, transform.position, Quaternion.identity);
        effect.transform.localScale = Vector3.one * (radius * 4 - 2);
        Collider2D[] overlap = Physics2D.OverlapCircleAll(transform.position, radius, interactionlayerMask);
        foreach (Collider2D collider in overlap)
        {
            if (collider.TryGetComponent<Rigidbody2D>(out Rigidbody2D rigidbody))
            {
                // Obstacles check
                Vector2 colliderCenter = collider.bounds.center;
                Vector2 colliderLeft = collider.bounds.center - collider.bounds.extents;
                Vector2 colliderRight = collider.bounds.center + collider.bounds.extents;

                RaycastHit2D leftObstacleHit = Physics2D.Linecast(transform.position, colliderLeft, obstaclelayerMask);
                RaycastHit2D centerObstacleHit = Physics2D.Linecast(transform.position, colliderCenter, obstaclelayerMask);
                RaycastHit2D rightObstacleHit = Physics2D.Linecast(transform.position, colliderRight, obstaclelayerMask);

                // hit check
                if (!centerObstacleHit && (!leftObstacleHit || !rightObstacleHit)) // central ray and atleast one side ray doesn't hit obstacle
                {
                    Vector2 direction = (collider.transform.position - transform.position).normalized;

                    // calculate and add force
                    float distance = Vector2.Distance(collider.transform.position, transform.position);
                    float coeficient = (radius - distance) / radius;
                    Vector3 force = direction * (velocityImpact * (coeficient * coeficient));

                    rigidbody.AddForce(force / coeficient, ForceMode2D.Impulse);

                    // deal damage
                    if (collider.TryGetComponent(out IHealthSystem health))
                    {
                        Vector2 directionToTarget = (collider.transform.position - transform.position).normalized;
                        health.ConsumeHp(damage, directionToTarget);
                    }
                }
            }
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, radius);
    }
}
