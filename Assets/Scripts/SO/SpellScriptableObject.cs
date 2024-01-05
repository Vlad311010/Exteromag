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
    // [HideInInspector] public Transform target;
    [HideInInspector] public Transform owner;

    [Header("Object Base")]
    public GameObject projectile;

    [Header("Effects")]
    public GameObject onCastParticles;
    public GameObject particles;
    public GameObject onHitParticles;
    public AudioClip hitSound;

    [Header("UI")]
    public Sprite spellIcon;
    public Color spellIconColor = Color.white;

    [Header("Cast Parameters")]
    public int castCost;
    public float cooldown;
    public bool parentWithCaster = false;
    public bool interuptOnSpellChange = true;
    public bool preventConstantCasting = false;

    // movement
    [Header("Movement Parameters")]
    public float speed;
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
    public KickbackAttributeData kickbackAttribute;
    public ReflectAttributeData reflectAttribute;
    public LimitedLifeTimeAttributeData limitedLifeTimeAttribute;
    public ChannelingAttributeData channelingAttribute;
    public DestroyTileAttributeData destroyTileAttribute;
    public ScaleAttributeData scaleAttribute;
    public PointCastAttributeData pointCastAttribute;

    // Spawn attributes data (namig conventions matters)
    public List<SpellSpawnAttribute> spawnAttributes;

    public DefaultSpawnAttributeData defaultSpawnAttribute;
    public RandomOffsetAttributeData randomOffsetAttribute;
    public ShotgunAttributeData shotgunAttribute;
    public MultipleAttributeData multipleAttribute;


}

// spell attributes
[Serializable]
public class ExplosionAttributeData
{
    public ExplosionAttributeData() { }
    public float radius;
    public int damage;
    public float velocityImpact;
    public LayerMask obstacleLayerMask;
    public LayerMask interactionLayerMask;
    public GameObject explosionEffect;
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

[Serializable]
public class KickbackAttributeData
{
    public float force;
}

[Serializable]
public class ReflectAttributeData
{
    public int reflectCount;
    public LayerMask layerMask;
    public LayerMask layerMaskToAdd;
    public bool reflectToCaster;
}

[Serializable]
public class LimitedLifeTimeAttributeData
{
    public float lifetime;
}

[Serializable]
public class ChannelingAttributeData
{
    public int slotIdx;
    public int channelingConst;
    public float channelingTick;
}

[Serializable]
public class DestroyTileAttributeData
{
    public LayerMask layerMask;
    public int radius;
    public GameObject destroyEffect;
}

[Serializable]
public class ScaleAttributeData
{
    public Vector2 scale;
}

[Serializable]
public class PointCastAttributeData
{
}

// spawn attributes

[Serializable]
public class DefaultSpawnAttributeData
{
    public float originOffset;
}

[Serializable]
public class RandomOffsetAttributeData
{
    public float angle;
}


[Serializable]
public class ShotgunAttributeData
{
    public float angle;
    public float range;
    public int projectilesCount;
    public bool evenDistribution;
}

[Serializable]
public class MultipleAttributeData
{
    public int projectilesCount;
    public float distanceBetweenProjectiles;
}

