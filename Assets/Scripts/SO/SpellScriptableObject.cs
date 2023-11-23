using System;
using System.Collections.Generic;
using UnityEngine;
using Enums;

[Serializable]
[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/Spell", order = 1)]
public class SpellScriptableObject : ScriptableObject
{
    [HideInInspector] public Vector2 casterPosition;
    [HideInInspector] public Vector2 castPoint;
    [HideInInspector] public Transform target;
    [HideInInspector] public Transform owner;

    [Header("References")]
    public GameObject projectile;
    public GameObject onCastParticles;
    public GameObject particles;
    public GameObject OnHitParticles;

    [Header("UI")]
    public Sprite spellIcon;
    public Color spellIconColor = Color.white;

    [Header("Cast Parameters")]
    public int castCost;
    public float cooldown;
    public bool castOnTarget;
    public bool castOnSelf;

    // movement
    [Header("Movement Parameters")]
    public float speed;
    public float acceleration;
    public float decceleration;
    public float deccelerationStart;
    public bool useDecceleration;

    public LayerMask collisionLayerMask;

    [Header("Attributes")]
    // Attributes data (namig conventions matters)
    public List<SpellAttribute> attributes;

    public ExplosionAttributeData explosionAttribute;
    public InstantAttributeData instantAttribute;
    public BounceAttributeData bounceAttribute;
    public RotateAroundTargetAttributeData rotateAroundTargetAttribute;
    public PenetrateAttributeData penetrateAttribute;
    public ManaDrainAttributeData manaDrainAttribute;
    public DamageAttributeData damageAttribute;

    // Spawn attributes data (namig conventions matters)
    public List<SpellSpawnAttribute> spawnAttributes;

    public DefaultSpawnAttributeData defaultSpawnAttribute;
    public RandomOffsetAttributeData randomOffsetAttribute;
    public ShotgunAttributeData shotgunAttribute;


}

// spell attributes
[Serializable]
public class ExplosionAttributeData
{
    public ExplosionAttributeData() { }
    public float radius;
    public float velocityImpact;
    public LayerMask obstacleLayerMask;
    public LayerMask interactionLayerMask;
}

[Serializable]
public class InstantAttributeData
{
    public float distanceLimit;
}

[Serializable]
public class BounceAttributeData
{
    public int bounceLimit;
    public LayerMask bounceLayer;
}

[Serializable]
public class RotateAroundTargetAttributeData
{
    public float speed;
    public float distanceFromAnchor;
}

[Serializable]
public class PenetrateAttributeData
{
    public int penetrateLimit;
    public LayerMask layerMask;
}

[Serializable]
public class ManaDrainAttributeData
{
    public int manaDrain;
    public LayerMask layerMask;
}

[Serializable]
public class DamageAttributeData
{
    public int damage;
}

// spawn attributes

[Serializable]
public class DefaultSpawnAttributeData
{
    public float distanceFromCaster;
}

[Serializable]
public class RandomOffsetAttributeData
{
    public float angle;
    public float range;
}


[Serializable]
public class ShotgunAttributeData
{
    public float angle;
    public float range;
    public int projectilesCount;
}

