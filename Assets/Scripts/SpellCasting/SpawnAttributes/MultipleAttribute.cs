using UnityEngine;
using Interfaces;
using Structs;
using Unity.Mathematics;

public class MultipleAttribute : ISpellSpawnAttribute
{
    private int projectilesCount;
    private float distanceBetweenProjectiles;
    public void GetAttributeParameters(SpellScriptableObject spell)
    {
        projectilesCount = spell.multipleAttribute.projectilesCount;
        distanceBetweenProjectiles = spell.multipleAttribute.distanceBetweenProjectiles;

    }

    public SpellSpawnData SetProjectileSpawnParameters(SpellSpawnData spawnData)
    {
        Vector2 direction = (spawnData.castPoint - spawnData.casterPosition).normalized;
        Vector2 rotationVector = Quaternion.Euler(0, 0, 0) * direction;
        Quaternion lookDirection = Quaternion.LookRotation(new Vector3(0, 0, 1), rotationVector.normalized);
        Vector2 vectorRightToDirection = Quaternion.Euler(0, 0, -90) * direction;
        float offset = distanceBetweenProjectiles;
        for (int i = 0; i < projectilesCount; i++)
        {
            int even = i % 2 == 0 ? -1 : 1;
            Vector2 position = spawnData.origin - even * vectorRightToDirection * offset;
            spawnData.AddProjectileData(position, lookDirection);
            offset = offset + math.clamp(even, 0, 1) * distanceBetweenProjectiles;
        }
        
        return spawnData;
    }
}
