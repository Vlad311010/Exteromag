using UnityEngine;
using Interfaces;
using Structs;
using System.Collections;
using UnityEngine.VFX;

public class SpellBase : MonoBehaviour
{
    private SFXPlayer sound;
    [SerializeField] public SpellScriptableObject spell { get; private set; }
    private ISpellAttribute[] attributes;
    private float lifeTime = 100f;
    private float destryoDelayTime;

    [HideInInspector] public LayerMask collisionLayerMask;

    public void Init(SpellScriptableObject spell)
    {
        sound = GetComponent<SFXPlayer>();
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
        if (spell.onHitParticles == null)
            return;

        Instantiate(spell.onHitParticles, transform.position, Quaternion.identity);
    }

    public void OnHit(CollisionData collisionData)
    {
        // spell impact
        sound.Play(spell.hitSound);
        
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
        if (sound.Clip != null)
        {
            GetComponent<Collider2D>().enabled = false;
            GetComponent<Rigidbody2D>().simulated = false;
            foreach (var vf in GetComponentsInChildren<VisualEffect>())
            {
                vf.enabled = false;
            }
            foreach (var sprite in GetComponentsInChildren<SpriteRenderer>())
            {
                sprite.enabled = false;
            }
            foreach (var trail in GetComponentsInChildren<TrailRenderer>())
            {
                trail.enabled = false;
            }


            StartCoroutine(DespawnCoroutine(sound.Clip.length));
        }
        else
            Destroy(gameObject);
    }

    IEnumerator DespawnCoroutine(float time)
    {
        yield return new WaitForSeconds(time);
        Destroy(gameObject);
    }
}
