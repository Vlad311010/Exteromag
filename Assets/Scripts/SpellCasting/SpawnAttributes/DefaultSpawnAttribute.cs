using Interfaces;
using Structs;

public class DefaultSpawnAttribute : ISpellSpawnAttribute
{
    private float distanceFromCaster;
    public void GetAttributeParameters(SpellScriptableObject spell)
    {
        distanceFromCaster = spell.defaultSpawnAttribute.originOffset;
    }

    public SpellSpawnData SetProjectileSpawnParameters(SpellSpawnData spellSpawnData)
    {
        spellSpawnData.origin = spellSpawnData.casterPosition + spellSpawnData.direction * distanceFromCaster;
        return spellSpawnData;
    }
}
