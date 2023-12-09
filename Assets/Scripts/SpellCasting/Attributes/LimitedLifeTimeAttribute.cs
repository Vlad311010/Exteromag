using Interfaces;
using Structs;
using UnityEngine;

public class LimitedLifeTimeAttribute : MonoBehaviour, ISpellAttribute
{
    private float lifetime;
    private float lifeTimeLeft;

    public void GetAttributeParameters(SpellScriptableObject spell)
    {
        lifetime = spell.limitedLifeTimeAttribute.lifetime;
        lifeTimeLeft = lifetime;
    }

    public void OnCastEvent() { }

    public void OnHitEvent(CollisionData collisionData) { }


    void Update()
    {
        if (lifeTimeLeft <= 0)
            GetComponent<SpellBase>().Despawn();

        lifeTimeLeft -= Time.deltaTime;
    }

    /*private void OnTriggerEnter2D(Collider2D collider)
    {
        lifeTimeLeft = 5;
    }*/
}
