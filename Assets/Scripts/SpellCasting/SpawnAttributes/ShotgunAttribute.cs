using Interfaces;
using Structs;

public class ShotgunAttribute : ISpellSpawnAttribute
{
    private float angle;
    private float range;
    private int projectilesCount;
    private bool evenDistribution = false;

    public void GetAttributeParameters(SpellScriptableObject spell)
    {
        angle = spell.shotgunAttribute.angle;
        range = spell.shotgunAttribute.range;
        projectilesCount = spell.shotgunAttribute.projectilesCount;
    }

    public SpellSpawnData SetProjectileSpawnParameters(SpellSpawnData spawnData)
    {
        float step = (angle * 2) / projectilesCount;
        float rotationAngle = evenDistribution ? -angle : UnityEngine.Random.Range(-angle, angle);
        for (int i = 0; i < projectilesCount; i++)
        {
            
            // float rotationAngle = UnityEngine.Random.Range(-angle, angle);
            UnityEngine.Vector2 direction = (spawnData.origin - spawnData.casterPosition).normalized;
            UnityEngine.Vector2 spawnOffset = UnityEngine.Quaternion.Euler(0, 0, rotationAngle) * direction * range;
            UnityEngine.Quaternion lookDirection = UnityEngine.Quaternion.LookRotation(new UnityEngine.Vector3(0, 0, 1), UnityEngine.Quaternion.Euler(0, 0, rotationAngle) * direction);
            spawnData.AddProjectileData(spawnData.casterPosition + spawnOffset, lookDirection);

            rotationAngle = evenDistribution ? rotationAngle + step : UnityEngine.Random.Range(-angle, angle);
            
        }

        return spawnData;
    }
}
