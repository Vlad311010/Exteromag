

using UnityEngine;


public class AIEffects : MonoBehaviour
{
    [SerializeField] GameObject splash;
    [SerializeField] GameObject manaParticles;

    [SerializeField] Color enemyColor;


    public void OnDeath()
    {
        Quaternion rotation = Quaternion.Euler(0, 0, Random.Range(0, 359));
        SpriteRenderer splashSprite = Instantiate(splash, transform.position, rotation).GetComponent<SpriteRenderer>();
        Instantiate(manaParticles, transform.position, Quaternion.identity);
        splashSprite.color = enemyColor;
    }


}
