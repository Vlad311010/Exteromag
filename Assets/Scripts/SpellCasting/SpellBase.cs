using UnityEngine;
using Interfaces;
using Structs;
using System.Collections;

public class SpellBase : MonoBehaviour
{
    [SerializeField] public SpellScriptableObject spell { get; private set; }
    private ISpellAttribute[] attributes;
    private float lifeTime = 100f;

    [HideInInspector] public LayerMask collisionLayerMask;
    

    public void Init(SpellScriptableObject spell)
    {
        this.spell = spell;
        attributes = GetAttributesList();
        collisionLayerMask = spell.collisionLayerMask;
        OnCast();
    }
    

    public void OnCast()
    {
        // instanciate particle
        SpawnCastParticles();
        SpawnProjectileParticles();
        // play sound

        AttributesOnCastEvent();
        StartCoroutine(DespawnCoroutine(lifeTime));
    }

    private void SpawnCastParticles()
    {
        if (spell.onCastParticles == null)
            return;

        Instantiate(spell.onCastParticles, transform.position, Quaternion.identity);
    }


    private void SpawnProjectileParticles()
    {
        if (spell.particles == null)
            return;

        Instantiate(spell.particles, transform.position, Quaternion.identity, this.gameObject.transform);
    }

    private void SpawnHitParticles()
    {
        if (spell.OnHitParticles == null)
            return;

        Instantiate(spell.OnHitParticles, transform.position, Quaternion.identity);
    }

    public void OnHit(CollisionData collisionData)
    {
        // instanciate particle
        // destroy object
        // spell impact

        AttributesOnHitEvent(collisionData);
    }

    private void AttributesOnCastEvent()
    {
        foreach (ISpellAttribute attribute in attributes)
        {
            attribute.OnCastEvent();
        }
    }
    private void AttributesOnHitEvent(CollisionData collisionData)
    {
        foreach (ISpellAttribute attribute in attributes)
        {
            attribute.OnHitEvent(collisionData);
        }
    }

    private ISpellAttribute[] GetAttributesList()
    {
        ISpellAttribute[] attributes = transform.GetComponents<ISpellAttribute>();
        return attributes;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collisionLayerMask == (collisionLayerMask | (1 << collision.gameObject.layer)))
        {
            OnHit(new CollisionData(collision.gameObject, collision.rigidbody, collision.contacts));
        }
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        ContactPoint2D contactPoint = new ContactPoint2D(); // TODO: can lead to problems. should populate contactPoint with proper data;
        if (collisionLayerMask == (collisionLayerMask | (1 << collider.gameObject.layer)))
        {
            OnHit(new CollisionData(collider.gameObject, collider.attachedRigidbody, new ContactPoint2D[] { contactPoint }));
        }
    }



    public void Despawn()
    {
        SpawnHitParticles();
        Destroy(this.gameObject);
    }

    IEnumerator DespawnCoroutine(float time)
    {
        yield return new WaitForSeconds(time);
        Despawn();
    }
}
