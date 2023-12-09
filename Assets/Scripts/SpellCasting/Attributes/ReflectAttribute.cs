using Interfaces;
using Structs;
using UnityEngine;

public class ReflectAttribute : MonoBehaviour, ISpellAttribute
{
    private int reflectCount;
    private LayerMask layerMask;
    private LayerMask layerMaskToAdd;
    private bool reflectToCaster;
    private Vector2 casterPosition;
    public void GetAttributeParameters(SpellScriptableObject spell)
    {
        reflectCount = spell.reflectAttribute.reflectCount;
        layerMask = spell.reflectAttribute.layerMask;
        layerMaskToAdd = spell.reflectAttribute.layerMaskToAdd;
        reflectToCaster = spell.reflectAttribute.reflectToCaster;
        casterPosition = spell.casterPosition;
    }

    public void OnCastEvent() {}
    public void OnHitEvent(CollisionData collisionData) {}

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (layerMask.CheckLayer(collider.gameObject.layer))
        {
            Vector2 direction; 
            if (reflectToCaster)
            {
                direction = (collider.GetComponent<SpellBase>().spell.casterPosition - (Vector2)collider.transform.position).normalized;
            }
            else
            {
                direction = ((Vector2)collider.transform.position - casterPosition).normalized;
            }
            collider.gameObject.layer = LayerMask.NameToLayer("Projectile");
            collider.GetComponent<ProjectileMovement>().direction = direction;
            collider.GetComponent<SpellBase>().collisionLayerMask |= layerMaskToAdd; // add new layers
            
        }
    }

}
