using Interfaces;
using Structs;

public class ShotgunAttribute : ISpellSpawnAttribute
{
    private float angle;
    private float range;
    private int projectilesCount;
    private bool evenDistribution;

    public void GetAttributeParameters(SpellScriptableObject spell)
    {
        angle = spell.shotgunAttribute.angle;
        range = spell.shotgunAttribute.range;
        projectilesCount = spell.shotgunAttribute.projectilesCount;
        evenDistribution = spell.shotgunAttribute.evenDistribution;
    }

    public SpellSpawnData SetProjectileSpawnParameters(SpellSpawnData spawnData)
    {
        float step = -angle / (projectilesCount - 1);
        float rotationAngle = evenDistribution ? angle/2 : UnityEngine.Random.Range(-angle/2, angle/2);
        for (int i = 0; i < projectilesCount; i++)
        {
            UnityEngine.Vector2 direction = (spawnData.castPoint - spawnData.casterPosition).normalized;
            UnityEngine.Vector2 spawnOffset = UnityEngine.Quaternion.Euler(0, 0, rotationAngle) * direction * range;
            UnityEngine.Quaternion lookDirection = UnityEngine.Quaternion.LookRotation(new UnityEngine.Vector3(0, 0, 1), spawnOffset.normalized);
            spawnData.AddProjectileData(spawnData.casterPosition + spawnOffset, lookDirection);
            rotationAngle = evenDistribution ? rotationAngle + step : UnityEngine.Random.Range(-angle/2, angle/2);
            
        }

        return spawnData;
    }
}
