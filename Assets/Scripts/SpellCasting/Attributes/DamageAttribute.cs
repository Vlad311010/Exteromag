using Interfaces;
using Structs;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
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
        if (collisionData.GameObject.TryGetComponent(out HealthSystem health))
        {
            health.ConsumeHp(damage);
        }
    }
}
