using Interfaces;
using Structs;

public class RandomOffsetAttribute : ISpellSpawnAttribute
{
    private float angle;

    public void GetAttributeParameters(SpellScriptableObject spell)
    {
       angle = spell.randomOffsetAttribute.angle;
    }

    public SpellSpawnData SetProjectileSpawnParameters(SpellSpawnData spawnData)
    {
        float rotationAngle = UnityEngine.Random.Range(-angle/2, angle/2);
        UnityEngine.Vector2 direction = (spawnData.castPoint - spawnData.casterPosition).normalized;
        UnityEngine.Vector2 rotationVector = UnityEngine.Quaternion.Euler(0, 0, rotationAngle) * direction;
        UnityEngine.Quaternion lookDirection = UnityEngine.Quaternion.LookRotation(new UnityEngine.Vector3(0, 0, 1), rotationVector.normalized);
        spawnData.AddProjectileData(spawnData.origin, lookDirection);
        return spawnData;
    }
}
