using Interfaces;
using Structs;
using UnityEngine;
using UnityEngine.Tilemaps;

public class PenetrateAttribute : MonoBehaviour, ISpellAttribute
{
    private int penetrateLimit;
    private LayerMask layerMask;

    public void GetAttributeParameters(SpellScriptableObject spell)
    {
        penetrateLimit = spell.penetrateAttribute.penetrateLimit;
        layerMask = spell.penetrateAttribute.layerMask;
    }

    public void OnCastEvent() { }
    

    public void OnHitEvent(CollisionData collisionData)
    {
        if (collisionData.IsNullValue()) return;
        
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        penetrateLimit--;
        if (penetrateLimit < 0 || !layerMask.CheckLayer(collision.gameObject.layer))
            GetComponent<SpellBase>().Despawn();

        GetComponent<Collider2D>().isTrigger = true;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        GetComponent<Collider2D>().isTrigger = false;
    }
}
