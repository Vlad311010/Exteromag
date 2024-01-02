using UnityEngine;
using Interfaces;
using Structs;

public class BounceAttribute : MonoBehaviour, ISpellAttribute
{
    private int bounceLimit;
    private LayerMask bounceLayer;
    
    private const float collisionStayTime = 0.05f;
    private float collisionStayTimer;

    public void GetAttributeParameters(SpellScriptableObject spell)
    {
        bounceLimit = spell.bounceAttribute.bounceLimit;
        bounceLayer = spell.bounceAttribute.bounceLayer;
        collisionStayTimer = collisionStayTime;
    }

    public void OnCastEvent() { }

    public void OnHitEvent(CollisionData collisionData)
    {
        if (collisionData.IsNullValue()) return;

        bounceLimit--;

        if (bounceLimit < 0 || !bounceLayer.CheckLayer(collisionData.GameObject.layer))
        {
            GetComponent<SpellBase>().Despawn();
        }
        else
        {
            // set move direction 
            Vector3 moventDir = transform.GetComponent<ProjectileMovement>().direction;
            Vector3 normalDir = collisionData.Contacts[0].normal;
            Vector3 reflectedVector = Vector3.Reflect(moventDir, normalDir).normalized;
            transform.GetComponent<ProjectileMovement>().direction = reflectedVector;

            // set rotation 
            Quaternion lookDirection = Quaternion.LookRotation(new Vector3(0, 0, 1), reflectedVector);
            transform.rotation = Quaternion.Euler(0, 0, lookDirection.eulerAngles.z);
        }
    }


    private void OnCollisionStay2D(Collision2D collision)
    {
        collisionStayTimer -= Time.deltaTime;
        if (collisionStayTimer <= 0)
        {
            OnHitEvent(new CollisionData(collision.gameObject, collision.rigidbody, collision.contacts));
            collisionStayTimer = collisionStayTime; 
        }
    }
}
