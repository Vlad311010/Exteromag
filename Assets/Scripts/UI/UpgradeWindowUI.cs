using System.Linq;
using UnityEngine;

public class UpgradeWindowUI : MonoBehaviour
{
    SpellSetSO[] spellSets;
    UpgradeSlotUI[] upgradeSlots;

    // private SpellSetSO selectedSet;
    private int selectedUpgrade;
    private int slotIdx;
    private CharacterInteraction characterInteraction;

    /*public void Activate()
    {
        characterInteraction = characterInteraction ?? GameObject.FindGameObjectWithTag("Player").GetComponent<CharacterInteraction>();
        spellSets = characterInteraction.containedSpells.Select(cs => cs.spellSet).ToArray();
        upgradeSlots = GetComponentsInChildren<UpgradeSlotUI>();

        for (int i = 0; i < upgradeSlots.Length; i++)
        {
            SpellSetSO spellSet = i < spellSets.Length ? spellSets[i] : null;
            upgradeSlots[i].Activate(spellSet, i);
        }
    }*/

    private void OnEnable()
    {
        characterInteraction = characterInteraction ?? GameObject.FindGameObjectWithTag("Player").GetComponent<CharacterInteraction>();
        spellSets = characterInteraction.containedSpells.Select(cs => cs.spellSet).ToArray();
        upgradeSlots = GetComponentsInChildren<UpgradeSlotUI>();

        for (int i = 0; i < upgradeSlots.Length; i++)
        {
            SpellSetSO spellSet = i < spellSets.Length ? spellSets[i] : null;
            upgradeSlots[i].Activate(spellSet, i);
        }
    }

    public void SetSelectedSpellUpgrade(int spellSet, int upgdateIdx)
    {
        slotIdx = spellSet;
        selectedUpgrade = upgdateIdx;
    }

    public void Upgrade()
    {
        characterInteraction.UpgradeSpell(slotIdx, selectedUpgrade);
    }
}
