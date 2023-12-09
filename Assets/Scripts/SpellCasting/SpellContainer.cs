using System.Collections;
using UnityEngine;

public class SpellContainer
{
    /*int level = 1;
    SpellScriptableObject level1;
    SpellScriptableObject level2;
    SpellScriptableObject level3;*/

    // data
    public SpellScriptableObject spell;
    public int slotIdx { get; private set; }

    // state
    public bool holdDown;
    public float cooldownTimer { get; private set; }
    public bool isInCooldown { get; private set; }

    public SpellContainer(SpellScriptableObject spell, int slotIdx)
    {
        this.slotIdx = slotIdx;
        this.spell = spell;

        holdDown = false;
        cooldownTimer = 0;
    }

    public IEnumerator SpellCooldown()
    {
        isInCooldown = true;
        cooldownTimer = spell.cooldown;
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
        return (spell.cooldown - cooldownTimer) / spell.cooldown;
    }

    public bool IsEmpty()
    {
        return spell == null;
    }
}
