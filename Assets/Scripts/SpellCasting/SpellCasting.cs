using Structs;
using System.Collections.Generic;
using UnityEngine;

public static class SpellCasting
{
    public static void Cast(Transform transform, SpellScriptableObject spell, AimSnapshot aimSnapshot)
    {
        spell = ScriptableObject.Instantiate(spell);

        SpellSpawnData spellSpawnData = new SpellSpawnData(transform.position, aimSnapshot.castPoint, aimSnapshot.castDirection);

        spellSpawnData = AttributesResolver.ResolveSpawnAttributes(spellSpawnData, spell);
        SpawnSpell(transform, spell, spellSpawnData);
    }

    private static void SpawnSpell(Transform transform, SpellScriptableObject spell, SpellSpawnData spellSpawnData)
    {
        List<SpellBase> spellObjects = new List<SpellBase>();

        if (spellSpawnData.projectilesPosition.Count != 0)
        {
            for (int i = 0; i < spellSpawnData.projectilesPosition.Count; i++)
            {
                SpellBase spellBase;
                if (spell.parentWithCaster)
                    spellBase = GameObject.Instantiate(spell.projectile, spellSpawnData.projectilesPosition[i], spellSpawnData.projectilesRotation[i], transform).GetComponent<SpellBase>();
                else 
                    spellBase = GameObject.Instantiate(spell.projectile, spellSpawnData.projectilesPosition[i], spellSpawnData.projectilesRotation[i]).GetComponent<SpellBase>();
                spellObjects.Add(spellBase);
            }
        }
        else
        {
            throw new System.Exception("EXX");
        }


        foreach (SpellBase spellObject in spellObjects)
        {
            spell = ScriptableObject.Instantiate(spell);
            spell.casterPosition = spellSpawnData.casterPosition;
            spell.castPoint = spellSpawnData.castPoint;
            spell.owner = transform;

            AttributesResolver.ResolveSpellAttributes(spellObject.gameObject, spell);
            spellObject.Init(spell);

            //add velocity

            if (spellObject.TryGetComponent(out ProjectileMovement projectileMovement))
            {
                if (spellSpawnData.forceMode == Enums.ForceApplyMode.LookDirection)
                {
                    projectileMovement.Init(spellObject.transform.up, spell.speed, spell.decceleration, spell.useDecceleration, spell.deccelerationStart);
                }
                else if (spellSpawnData.forceMode == Enums.ForceApplyMode.CastPoint)
                {
                    Vector2 direction = (spellSpawnData.castPoint - (Vector2)spellObject.transform.position).normalized;
                    projectileMovement.Init(direction, spell.speed, spell.decceleration, spell.useDecceleration, spell.deccelerationStart);
                }
            }
        }
    }
}
