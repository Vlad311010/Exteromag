using Enums;
using Interfaces;
using Structs;
using System;
using UnityEngine;

public static class AttributesResolver
{
    private static void ResolveSpellAttribute(GameObject spellProjectile, SpellScriptableObject spell, SpellAttribute selectedAttribute)
    {
        string className = Enum.GetName(typeof(SpellAttribute), selectedAttribute) + "Attribute";
        Type componentType = Type.GetType(className);
        ISpellAttribute attribute = spellProjectile.AddComponent(componentType) as ISpellAttribute;
        attribute.GetAttributeParameters(spell);
    }

    public static void ResolveSpellAttributes(GameObject spellProjectile, SpellScriptableObject spell)
    {
        if (spell.attributes.Count == 0) return;

        for (int i = 0; i < spell.attributes.Count; i++)
        {
            ResolveSpellAttribute(spellProjectile, spell, spell.attributes[i]);
        }
    }


    private static SpellSpawnData ResolveSpawnAttribute(SpellSpawnData spellSpawnData, SpellScriptableObject spell, SpellSpawnAttribute attributeName)
    {
        string className = Enum.GetName(typeof(SpellSpawnAttribute), attributeName) + "Attribute";
        Type componentType = Type.GetType(className);
        ISpellSpawnAttribute attributeInstance = (ISpellSpawnAttribute)Activator.CreateInstance(componentType);
        attributeInstance.GetAttributeParameters(spell);
        spellSpawnData = attributeInstance.SetProjectileSpawnParameters(spellSpawnData);
        return spellSpawnData;
    }

    public static SpellSpawnData ResolveSpawnAttributes(SpellSpawnData spellSpawnData, SpellScriptableObject spell)
    {
        for (int i = 0; i < spell.spawnAttributes.Count; i++)
        {
            spellSpawnData = ResolveSpawnAttribute(spellSpawnData, spell, spell.spawnAttributes[i]);
        }
        return spellSpawnData;
    }
}

