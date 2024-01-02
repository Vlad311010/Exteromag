

using UnityEngine;
using UnityEngine.VFX;

public class AIEffects : MonoBehaviour
{
    [SerializeField] GameObject splash;
    [SerializeField] GameObject manaParticles;
    [SerializeField] GameObject onDeathEffect;

    [SerializeField] Color enemyColor;
    [ColorUsageAttribute(true, true)]
    [SerializeField] Color deathEffectColor;




    public void OnDeath()
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


}
