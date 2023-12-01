using UnityEngine;
using Interfaces;
using Structs;

public class BounceAttribute : MonoBehaviour, ISpellAttribute
{
    private int bounceLimit;
    private LayerMask bounceLayer;

    public void GetAttributeParameters(SpellScriptableObject spell)
    {
        bounceLimit = spell.bounceAttribute.bounceLimit;
        bounceLayer = spell.bounceAttribute.bounceLayer;
    }

    public void OnCastEvent() { }

    public void OnHitEvent(CollisionData collisionData)
    {
        if (collisionData.IsNullValue()) return;

        bounceLimit--;

        if (bounceLimit < 0 || !bounceLayer.CheckLayer(collisionData.GameObject.layer))
        {
            GetComponent<SpellBase>().Despawn();
        }
        else
        {
            Vector3 moventDir = transform.GetComponent<ProjectileMovement>().direction;
            Vector3 normalDir = collisionData.Contacts[0].normal;
            Vector3 reflectedVector = Vector3.Reflect(moventDir, normalDir).normalized;
            transform.GetComponent<ProjectileMovement>().direction = reflectedVector;
        }
    }
}
