using Interfaces;
using Structs;
using UnityEngine;

public class KickbackAttribute : MonoBehaviour, ISpellAttribute
{
    private float force;
    public void GetAttributeParameters(SpellScriptableObject spell)
    {
        force = spell.kickbackAttribute.force;
    }

    public void OnCastEvent() { }

    public void OnHitEvent(CollisionData collisionData)
    {
        Vector2 direction = (Vector2)collisionData.GameObject.transform.position - collisionData.Contacts[0].point;
        collisionData.GameObject.GetComponent<Rigidbody2D>().AddForce(direction * force, ForceMode2D.Impulse);
    }
}
