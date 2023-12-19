using Interfaces;
using Structs;
using System.Collections;
using UnityEngine;

public class ChannelingAttribute : MonoBehaviour, ISpellAttribute
{
    private int slotIdx;
    private bool channeling = true;
    private int channelingConst;
    private float channelingTick;

    ManaPool manaPool;
    public void GetAttributeParameters(SpellScriptableObject spell) 
    {
        slotIdx = spell.channelingAttribute.slotIdx;
        channelingConst = spell.channelingAttribute.channelingConst;
        channelingTick = spell.channelingAttribute.channelingTick;

        GameEvents.current.onSpellCastButtonHold += HoldState;
        
        if (spell.owner.TryGetComponent(out manaPool))
            StartCoroutine(ManaConsuming());
    }

    public void OnCastEvent() { }
    public void OnHitEvent(CollisionData collisionData) { }

    private void HoldState(bool holding, int slotIdx) 
    {
        if (this.slotIdx != slotIdx) return;
       
        channeling = holding;
    }

    private void Update()
    {
        if (!channeling)
            GetComponent<SpellBase>().Despawn();

        
    }

    IEnumerator ManaConsuming()
    {
        yield return new WaitForSeconds(channelingTick);
        if (!manaPool.HaveEnaughtMp(channelingConst))
            channeling = false;

        manaPool.Consume(channelingConst);
        StartCoroutine(ManaConsuming());
    }
}
