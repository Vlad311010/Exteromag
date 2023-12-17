using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UpgradeSlotUI : MonoBehaviour, IPointerEnterHandler
{
    [SerializeField] UpgradeWindowUI upgradeWindow;
    [SerializeField] TMPro.TMP_Text description;
    [SerializeField] Button upgradeButton;
    [SerializeField] Image[] images;

    private SpellSetSO spellSet;
    private int slotIdx;

    public void Activate(SpellSetSO spellSetToUse, int slotIdx)
    {
        upgradeButton.interactable = false;
        if (spellSetToUse == null || spellSetToUse.upgrades.Count == 0)
        {
            gameObject.SetActive(false);
            return;
        }

        spellSet = spellSetToUse;
        this.slotIdx = slotIdx;
        images = GetComponentsInChildren<Image>();
        for (int i = 0; i < images.Length; i++)
        {
            images[i].gameObject.SetActive(i < spellSet.upgrades.Count ? true : false);
        }
    }
    

    public void ShowUpgradeDescription(int idx)
    {
        description.text = spellSet.upgrades[idx].description;  

    }

    public void SetSelectedSpellUpgrade(int upgradeIdx)
    {
        upgradeWindow.SetSelectedSpellUpgrade(slotIdx, upgradeIdx);
        upgradeButton.interactable = true;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        // eventData.pointerEnter.GetComponent<Button>().
        // Debug.Log("Mouse entered the button! " + eventData.pointerEnter.name);
    }

}
