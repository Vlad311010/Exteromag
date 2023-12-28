

using UnityEngine;


public class AIEffects : MonoBehaviour
{
    [SerializeField] GameObject splash;
    [SerializeField] GameObject manaParticles;
    [SerializeField] GameObject onDeathEffect;

    [SerializeField] Color enemyColor;


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
            Instantiate(onDeathEffect, transform.position, transform.rotation);
        }

        Instantiate(manaParticles, transform.position, Quaternion.identity);
    }


}
