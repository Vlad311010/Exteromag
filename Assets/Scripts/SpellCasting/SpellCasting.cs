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
                SpellBase spellBase = GameObject.Instantiate(spell.projectile, spellSpawnData.projectilesPosition[i], spellSpawnData.projectilesRotation[i]).GetComponent<SpellBase>();
                spellObjects.Add(spellBase);
            }
        }
        else
        {
            Vector2 direction = (spellSpawnData.castPoint - spellSpawnData.origin).normalized;
            Quaternion lookDirection = Quaternion.LookRotation(new Vector3(0, 0, 1), direction);
            SpellBase spellBase = GameObject.Instantiate(spell.projectile, spellSpawnData.origin, lookDirection).GetComponent<SpellBase>();
            spellObjects.Add(spellBase);
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
            if (spellSpawnData.forceMode == Enums.ForceApplyMode.LookDirection)
            {
                spellObject.GetComponent<ProjectileMovement>().Init(spellObject.transform.up, spell.speed, spell.acceleration, spell.decceleration, spell.useDecceleration, spell.deccelerationStart);
            }
            else if (spellSpawnData.forceMode == Enums.ForceApplyMode.CastPoint)
            {
                Vector2 direction = (spellSpawnData.castPoint - (Vector2)spellObject.transform.position).normalized;
                spellObject.GetComponent<ProjectileMovement>().Init(direction, spell.speed, spell.acceleration, spell.decceleration, spell.useDecceleration, spell.deccelerationStart);
            }

        }
    }
}
