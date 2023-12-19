using Interfaces;
using Structs;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScaleAttribute : MonoBehaviour, ISpellAttribute
{
    private Vector2 scale;
    public void GetAttributeParameters(SpellScriptableObject spell)
    {
        scale = spell.scaleAttribute.scale;
    }

    public void OnCastEvent()
    {
        transform.localScale = scale;
    }

    public void OnHitEvent(CollisionData collisionData) { }
}