using Interfaces;
using Structs;
using UnityEngine;

public class RandomOffsetAttribute : ISpellSpawnAttribute
{
    private float angle;

    public void GetAttributeParameters(SpellScriptableObject spell)
    {
       angle = spell.randomOffsetAttribute.angle;
    }

    public SpellSpawnData SetProjectileSpawnParameters(SpellSpawnData spawnData)
    {
        Vector2 direction;
        Vector2 rotationVector;
        Quaternion lookDirection;
        if (spawnData.ProjectileCount == 0) // create projectile with random offset
        {
            float rotationAngle = Random.Range(-angle / 2, angle / 2);
            direction = (spawnData.castPoint - spawnData.casterPosition).normalized;
            rotationVector = Quaternion.Euler(0, 0, rotationAngle) * direction;
            lookDirection = Quaternion.LookRotation(new UnityEngine.Vector3(0, 0, 1), rotationVector.normalized);
            spawnData.AddProjectileData(spawnData.origin, lookDirection);
        }
        else // set random offset for projectiles in spawnData
        {
            for (int i = 0; i < spawnData.ProjectileCount; i++)
            {
                float rotationAngle = Random.Range(-angle / 2, angle / 2);
                spawnData.ModifieProjectileRotationData(i, spawnData.projectilesRotation[i] * Quaternion.Euler(0, 0, rotationAngle));
            }
        }
        return spawnData;
    }
}
