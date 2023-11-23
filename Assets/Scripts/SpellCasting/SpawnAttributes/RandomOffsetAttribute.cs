using Interfaces;
using Structs;

public class RandomOffsetAttribute : ISpellSpawnAttribute
{
    private float angle;
    private float range;

    public void GetAttributeParameters(SpellScriptableObject spell)
    {
       angle = spell.randomOffsetAttribute.angle;
       range = spell.randomOffsetAttribute.range;
    }

    public SpellSpawnData SetProjectileSpawnParameters(SpellSpawnData spawnData)
    {
        float rotationAngle = UnityEngine.Random.Range(-angle, angle);
        UnityEngine.Vector2 direction = (spawnData.origin - spawnData.casterPosition).normalized;
        UnityEngine.Vector2 spawnOffset = UnityEngine.Quaternion.Euler(0, 0, rotationAngle) * direction * range;
        spawnData.origin = spawnData.origin + spawnOffset;
        return spawnData;
    }
}
