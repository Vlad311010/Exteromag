using System.Collections;
using UnityEngine;

public class SpellContainer
{
    // data
    public SpellSetSO spellSet;
    public int slotIdx { get; private set; }

    // state
    public bool holdDown;
    public float cooldownTimer { get; private set; }
    public bool isInCooldown { get; private set; }

    public SpellScriptableObject spell { get => spellSet.spell; }
    public int castCost { get => spellSet.spell.castCost; }
    public float cooldown { get => spellSet.spell.cooldown; }


    public SpellContainer(SpellSetSO spellSet, int slotIdx)
    {
        this.slotIdx = slotIdx;
        this.spellSet = GameObject.Instantiate(spellSet);

        holdDown = false;
        cooldownTimer = 0;
    }

    public IEnumerator SpellCooldown()
    {
        isInCooldown = true;
        cooldownTimer = cooldown;
        while (cooldownTimer >= 0)
        {
            cooldownTimer -= Time.deltaTime;
            GameEvents.current.SpellCooldownValueChange(GetCooldownPercent(), slotIdx);
            yield return new WaitForEndOfFrame();
        }
        isInCooldown = false;
    }

    private float GetCooldownPercent()
    {
        return (cooldown - cooldownTimer) / cooldown;
    }

    public bool IsEmpty()
    {
        return spellSet == null;
    }
}
