using UnityEngine;
using Interfaces;
using Structs;
using System.Collections;
using UnityEngine.VFX;
using UnityEngine.Tilemaps;

public class SpellBase : MonoBehaviour
{
    private SFXPlayer sound;
    [SerializeField] public SpellScriptableObject spell { get; private set; }
    private ISpellAttribute[] attributes;
    private float lifeTime = 100f;
    private float destryoDelayTime;

    private Vector2 lastCollisionNormal = Vector2.zero;
    private Transform lastCollisionTransform;

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
        SpawnCastParticles();
        SpawnProjectileParticles();
        sound.Play(spell.castSound);

        AttributesOnCastEvent();
        StartCoroutine(DespawnCoroutine(lifeTime));
    }

    private void SpawnCastParticles()
    {
        if (spell.onCastEffect == null)
            return;

        Instantiate(spell.onCastEffect, transform.position, Quaternion.identity);
    }


    private void SpawnProjectileParticles()
    {
        if (spell.particles == null)
            return;

        Instantiate(spell.particles, transform.position, Quaternion.identity, this.gameObject.transform);
    }

    private void SpawnHitParticles()
    {
        if (spell.onHitEffect == null)
            return;


        GameObject hitEffect = spell.onHitEffectTilemap != null && lastCollisionTransform.gameObject.TryGetComponent(out Tilemap _) ? spell.onHitEffectTilemap : spell.onHitEffect;
        Quaternion lookDirection = Quaternion.LookRotation(new Vector3(0, 0, 1), -lastCollisionNormal);
        Quaternion rotationToNoraml = Quaternion.Euler(0, 0, lookDirection.eulerAngles.z);
        GameObject effect = Instantiate(hitEffect, transform.position, rotationToNoraml);
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
        lastCollisionNormal = collision.contacts[0].normal;
        lastCollisionTransform = collision.transform;
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
