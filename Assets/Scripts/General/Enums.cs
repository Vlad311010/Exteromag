namespace Enums
{
    public enum SpellAttribute
    {
        Explosion = 0,
        Instant = 1,
        Bounce = 2,
        RotateAroundTarget = 3,
        Penetrate = 4,
        ManaDrain = 5,
        Damage = 6,
        Kickback = 7,
        Reflect = 8,
        LimitedLifeTime = 9,
        Channeling = 10,
        DestroyTile = 11,
        Scale = 12,
        PointCast = 13
    }

    public enum SpellSpawnAttribute
    {
        DefaultSpawn = 0,
        UnderCursor = 1,
        RandomOffset = 2,
        Shotgun = 3,
        Multiple = 4
    }

    public enum ForceApplyMode
    {
        CastPoint = 0,
        LookDirection = 1
    }

    public enum DamageType
    {
        Regular = 0,
        Fire = 1,
        Ice = 2
    }

    public enum SpellCastTarget
    {
        None = 0,
        Point = 1,
        Transform = 2
    }


}
