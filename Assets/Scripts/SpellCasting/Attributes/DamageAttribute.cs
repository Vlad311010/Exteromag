using Interfaces;
using Structs;
using UnityEngine;

public class DamageAttribute : MonoBehaviour, ISpellAttribute
{
    private int damage;
    public void GetAttributeParameters(SpellScriptableObject spell)
    {
        damage = spell.damageAttribute.damage;
    }

    public void OnCastEvent() { }

    public void OnHitEvent(CollisionData collisionData)
    {
        if (collisionData.IsNullValue()) return;

        if (collisionData.GameObject.TryGetComponent(out IHealthSystem health))
        {
            Vector2 directionToTarget = (collisionData.GameObject.transform.position - transform.position).normalized;
            health.ConsumeHp(damage, directionToTarget);
        }
    }
}
