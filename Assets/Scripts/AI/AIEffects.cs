

using System.Collections;
using UnityEngine;
using UnityEngine.VFX;

public class AIEffects : MonoBehaviour
{
    [Header("Switches")]
    [SerializeField] bool skipHitVisualEffect;
    // [SerializeField] bool skipHitSoundEffect;
    [SerializeField] bool skipDeathVisualEffect;
    // [SerializeField] bool skipDeathSoundEffect;


    [Header("Hit effect")]
    [SerializeField] Material hitMaterial;
    [SerializeField] Material defaultMaterial;
    [SerializeField] float hitHighlightTime;
    [SerializeField] AudioClip hitSound;

    [Header("Death effect")]
    [SerializeField] GameObject splash;
    [SerializeField] GameObject manaParticles;
    [SerializeField] GameObject onDeathEffect;
    [SerializeField] AudioClip deathSound;

    [SerializeField] Color enemyColor;
    [ColorUsageAttribute(true, true)]
    [SerializeField] Color deathEffectColor;

    

    private SpriteRenderer[] sprites;
    private SFXPlayer sound;
    private Coroutine hitHighlightCoroutine;
    

    private void Start()
    {
        sprites = GetComponentsInChildren<SpriteRenderer>();
        sound = GetComponent<SFXPlayer>();

    }


    public void OnHit()
    {
        // visual 
        if (!skipHitVisualEffect)
        {
            if (hitHighlightCoroutine != null)
            {
                StopCoroutine(hitHighlightCoroutine);
            }
            hitHighlightCoroutine = StartCoroutine(HitHighlight());
        }

        // sound
        sound.Play(hitSound);
    }

    IEnumerator HitHighlight()
    {
        foreach (SpriteRenderer sprite in sprites)
        {
            if (sprite != null)
                sprite.material = hitMaterial;
        }

        yield return new WaitForSeconds(hitHighlightTime);

        foreach (SpriteRenderer sprite in sprites)
        {
            if (sprite != null)
                sprite.material = defaultMaterial;
        }
    }
    public void OnDeath()
    {
        // visual effects
        if (!skipDeathVisualEffect)
        {
            Quaternion rotation = Quaternion.Euler(0, 0, Random.Range(0, 359));
            if (splash != null)
            {
                SpriteRenderer splashSprite = Instantiate(splash, transform.position, rotation).GetComponent<SpriteRenderer>();
                splashSprite.color = enemyColor;
            }

            if (onDeathEffect != null)
            {
                GameObject deathEffect = Instantiate(onDeathEffect, transform.position, transform.rotation);
                foreach (VisualEffect vfx in deathEffect.GetComponentsInChildren<VisualEffect>())
                {
                    vfx.SetVector4("Color", deathEffectColor);
                }
            }
            Instantiate(manaParticles, transform.position, Quaternion.identity);
        }

        // sound effects
        sound.Play(deathSound);
    }
}
