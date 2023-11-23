
using Interfaces;
using Structs;
using UnityEngine;

public class ManaDrainAttribute : MonoBehaviour, ISpellAttribute
{
    private int manaDrain;
    private LayerMask layerMask;
    private Transform owner;
    public void GetAttributeParameters(SpellScriptableObject spell)
    {
        manaDrain = spell.manaDrainAttribute.manaDrain;
        layerMask = spell.manaDrainAttribute.layerMask;
        owner = spell.owner;
    }

    public void OnCastEvent() { }

    public void OnHitEvent(CollisionData collisionData)
    {
        if (!layerMask.CheckLayer(collisionData.GameObject.layer)) return;

        owner.GetComponent<ManaPool>().Consume(-manaDrain); 
    }

}
