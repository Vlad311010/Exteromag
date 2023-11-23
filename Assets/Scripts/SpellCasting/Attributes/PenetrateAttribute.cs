using Interfaces;
using Structs;
using UnityEngine;

public class PenetrateAttribute : MonoBehaviour, ISpellAttribute
{
    private int penetrateLimit;
    private LayerMask layerMask;

    public void GetAttributeParameters(SpellScriptableObject spell)
    {
        penetrateLimit = spell.penetrateAttribute.penetrateLimit;
        layerMask = spell.penetrateAttribute.layerMask;
    }

    public void OnCastEvent() { }
    

    public void OnHitEvent(CollisionData collisionData)
    {
        if (collisionData.IsNullValue()) return;

        penetrateLimit--;
        // if (penetrateLimit < 0 || layerMask != (layerMask | (1 << collisionData.GameObject.layer)))
        if (penetrateLimit < 0 || !layerMask.CheckLayer(collisionData.GameObject.layer))
        {
            GetComponent<SpellBase>().Despawn();
        }
        else
        {
            Physics2D.IgnoreCollision(GetComponent<Collider2D>(), collisionData.GameObject.GetComponent<Collider2D>(), true);
            // GetComponent<ProjectileMovement>().direction = collisionData.Contacts[0].relativeVelocity.normalized;
        }
    }
}
