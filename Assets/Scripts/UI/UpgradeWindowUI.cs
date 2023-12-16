using UnityEngine;

public class UpgradeWindowUI : MonoBehaviour
{
    [SerializeField] SpellSetSO[] spellSets;

    UpgradeSlotUI[] upgradeSlots;

    private SpellSetSO selectedSet;
    private int selectedUpgrade;
    
    public void Activate()
    {
        upgradeSlots = GetComponentsInChildren<UpgradeSlotUI>();
        for (int i = 0; i < upgradeSlots.Length; i++)
        {
            SpellSetSO spellSet = i < spellSets.Length ? spellSets[i] : null;
            upgradeSlots[i].Activate(spellSet);
        }
    }

    public void SetSelectedSpellUpgrade(SpellSetSO spellSet, int upgdateIdx)
    {
        selectedSet = spellSet;
        selectedUpgrade = upgdateIdx;
    }

    public void Upgrade()
    {
        Debug.Log(selectedSet + " " + selectedUpgrade);
    }
}
