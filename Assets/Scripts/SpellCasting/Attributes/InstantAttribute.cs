using UnityEngine;
using Interfaces;
using Structs;

public class InstantAttribute : MonoBehaviour, ISpellAttribute
{
    private Vector2 castPoint;
    private float distanceLimit;
    
    private Vector2 originalPosition;

    public void GetAttributeParameters(SpellScriptableObject spell)
    {
        castPoint = spell.castPoint;
        distanceLimit = spell.instantAttribute.distanceLimit;
    }


    public void OnCastEvent()
    {
        originalPosition = transform.position;
        Vector2 curentPosition = transform.position;
        float distanceToCastPosition = Vector2.Distance(curentPosition, castPoint);
        if (distanceToCastPosition > distanceLimit)
        {
            castPoint -= (castPoint - curentPosition).normalized * (distanceToCastPosition - distanceLimit);
        }

        transform.position = castPoint;
        SpellBase spellBase = GetComponent<SpellBase>();
        spellBase.OnHit(CollisionData.Empty);
        spellBase.Despawn();
        
    }
    
    public void OnHitEvent(CollisionData collisionData) { }

    private void OnDrawGizmos()
    {
        Debug.DrawLine(originalPosition, castPoint, Color.red);
    }

}
