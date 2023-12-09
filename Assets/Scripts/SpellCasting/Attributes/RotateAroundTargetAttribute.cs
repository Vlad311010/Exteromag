using UnityEngine;
using Interfaces;
using Structs;
using System;

public class RotateAroundTargetAttribute : MonoBehaviour, ISpellAttribute
{
    Transform anchor;
    float speed;
    float distanceFromAnchor;

    private Vector3 startingDirectionFromAnchor;
    private float angleOffset;

    public void GetAttributeParameters(SpellScriptableObject spell)
    {
        throw new NotImplementedException("Casting on target isn't implemented");
        speed = spell.rotateAroundTargetAttribute.speed;
        distanceFromAnchor = spell.rotateAroundTargetAttribute.distanceFromAnchor;
        // anchor = spell.target;
    }

    public void OnCastEvent() 
    {
        startingDirectionFromAnchor = (transform.position - anchor.position).normalized; 
    }

    void Update()
    {
        if (!anchor) return;

        angleOffset += (speed * Time.deltaTime) % 360;

        Vector3 directionFromAnchro = CalculateDirectionAfterRotation(angleOffset);
        transform.position = anchor.transform.position + directionFromAnchro * distanceFromAnchor; // set position reletive to current angleOffset
        transform.rotation = Quaternion.LookRotation(Vector3.forward, directionFromAnchro); // set look direction

    }

    public void OnHitEvent(CollisionData collisionData) { }

    private Vector3 CalculateDirectionAfterRotation(float angle)
    {
        return Quaternion.AngleAxis(angle, Vector3.forward) * startingDirectionFromAnchor;
    }
}
