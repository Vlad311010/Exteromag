using System.Linq;
using UnityEngine;

public class UpgradeWindowUI : MonoBehaviour
{
    SpellSetSO[] spellSets;
    UpgradeSlotUI[] upgradeSlots;

    private int selectedUpgrade;
    private int slotIdx;
    private CharacterInteraction characterInteraction;

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
        for (int i = 0; i < upgradeSlots.Length; i++)
        {
            upgradeSlots[i].SetActiveSprite(i == spellSet ? upgdateIdx : -1);
        }
        Debug.Log(spellSet + " " + upgdateIdx + " " + upgradeSlots.Length + " " + spellSets.Length);
    }

    public void Upgrade()
    {
        characterInteraction.UpgradeSpell(slotIdx, selectedUpgrade);
    }
}
