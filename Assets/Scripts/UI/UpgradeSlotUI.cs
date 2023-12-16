using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UpgradeSlotUI : MonoBehaviour, IPointerEnterHandler
{
    [SerializeField] UpgradeWindowUI upgradeWindow;
    [SerializeField] Image[] images;
    [SerializeField] TMPro.TMP_Text description;

    private SpellSetSO spellSet;

    public void Activate(SpellSetSO spellSetToUse)
    {
        if (spellSetToUse == null || spellSetToUse.upgrades.Count == 0)
        {
            gameObject.SetActive(false);
            return;
        }

        spellSet = spellSetToUse;
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
        upgradeWindow.SetSelectedSpellUpgrade(spellSet, upgradeIdx);
    }

    
    public void OnPointerEnter(PointerEventData eventData)
    {
        // eventData.pointerEnter.GetComponent<Button>().
        Debug.Log("Mouse entered the button! " + eventData.pointerEnter.name);
    }
}
