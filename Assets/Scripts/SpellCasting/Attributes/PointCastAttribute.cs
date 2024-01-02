using UnityEngine;
using Interfaces;
using Structs;

public class PointCastAttribute : MonoBehaviour, ISpellAttribute
{

    private Vector2 origin;
    private Vector2 castpoint;
    private float distanceLimit;

    public void GetAttributeParameters(SpellScriptableObject spell)
    {
        origin = transform.position;
        castpoint = spell.castPoint;
        distanceLimit = Vector2.Distance(origin, castpoint);
    }

    public void OnCastEvent() { }

    private void FixedUpdate()
    {
        float distance = Vector2.Distance(origin, transform.position);
        if (distance > distanceLimit)
        {
            SpellBase spellBase = GetComponent<SpellBase>();
            spellBase.OnHit(CollisionData.Empty);
            spellBase.Despawn();
        }
    }

    public void OnHitEvent(CollisionData collisionData) { }
}
