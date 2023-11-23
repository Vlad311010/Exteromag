using UnityEngine;
using Interfaces;
using Structs;

public class ExplosionAttribute : MonoBehaviour, ISpellAttribute
{
    private float radius;
    private float velocityImpact;
    private LayerMask obstaclelayerMask;
    private LayerMask interactionlayerMask;

    public void GetAttributeParameters(SpellScriptableObject spell)
    {
        radius = spell.explosionAttribute.radius;
        velocityImpact = spell.explosionAttribute.velocityImpact;
        obstaclelayerMask = spell.explosionAttribute.obstacleLayerMask;
        interactionlayerMask = spell.collisionLayerMask;
    }

    public void OnCastEvent()
    {
        
    }

    public void OnHitEvent(CollisionData collisionData)
    {
        Collider2D[] overlap = Physics2D.OverlapCircleAll(transform.position, radius, interactionlayerMask);
        foreach (Collider2D collider in overlap)
        {
            if (collider.TryGetComponent<Rigidbody2D>(out Rigidbody2D rigidbody))
            {
                // Obstacles check
                Vector3 colliderCenter = collider.bounds.center;
                Vector3 colliderLeft = collider.bounds.center + new Vector3(collider.bounds.min.x, 0);
                Vector3 colliderRight = collider.bounds.center + new Vector3(collider.bounds.max.x, 0);

                RaycastHit2D leftObstacleHit = Physics2D.Linecast(transform.position, colliderLeft, obstaclelayerMask);
                RaycastHit2D centerObstacleHit = Physics2D.Linecast(transform.position, colliderCenter, obstaclelayerMask);
                RaycastHit2D rightObstacleHit = Physics2D.Linecast(transform.position, colliderRight, obstaclelayerMask);

                if (!centerObstacleHit && (!leftObstacleHit || !rightObstacleHit)) // central ray and atleast one side ray doesn't hit obstacle
                {
                    Vector2 direction = (collider.transform.position - transform.position).normalized;

                    // calculate force
                    float distance = Vector2.Distance(collider.transform.position, transform.position);
                    float coeficient = (radius - distance) / radius;
                    Vector3 force = direction * (velocityImpact * (coeficient * coeficient));

                    rigidbody.AddForce(force / coeficient, ForceMode2D.Impulse);
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
