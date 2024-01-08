using System.Collections.Generic;
using UnityEngine;
using Enums;
using System.Collections;

namespace Structs
{
    public struct AimSnapshot
    {
        public Vector2 castPoint;
        public Vector2 castDirection;

        public AimSnapshot(Vector2 castPoint, Vector2 castDirection)
        {
            this.castPoint = castPoint;
            this.castDirection = castDirection;
        }
    }

    public struct CollisionData
    {
        public CollisionData(GameObject gameObject, Rigidbody2D rigidbody, ContactPoint2D[] contacts)
        {
            GameObject = gameObject;
            Rigidbody = rigidbody;
            Contacts = contacts;
        }

        public GameObject GameObject { get; }
        public Rigidbody2D Rigidbody { get; }
        public ContactPoint2D[] Contacts { get; }

        public static CollisionData Empty { get { return new CollisionData(null, null, null); } }

        public bool IsNullValue()
        {
            return this.Equals(Empty);
        }
    }

    public struct SpellSpawnData
    {
        public Vector2 casterPosition;
        public Vector2 castPoint;
        public Vector2 direction;
        public Vector2 origin;
        public ForceApplyMode forceMode;
        public List<Vector2> projectilesPosition { get; }
        public List<Quaternion> projectilesRotation { get; }

        public int ProjectileCount => projectilesPosition.Count;

        public SpellSpawnData(Vector2 casterPosition, Vector2 castPoint, Vector2 direction)
        {
            this.casterPosition = casterPosition;
            this.castPoint = castPoint;
            this.direction = direction;
            origin = casterPosition;
            forceMode = ForceApplyMode.LookDirection;
            projectilesPosition = new List<Vector2>();
            projectilesRotation = new List<Quaternion>();
        }

        public void AddProjectileData(Vector2 pos, Quaternion rotation)
        {
            projectilesPosition.Add(pos);
            projectilesRotation.Add(rotation);
        }
        public void ModifieProjectileRotationData(int idx, Quaternion rotation)
        {
            // projectilesPosition.Add(pos);
            projectilesRotation[idx] = rotation;
        }

    }

}
