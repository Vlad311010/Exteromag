using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UpgradeSlotUI : MonoBehaviour, IPointerEnterHandler
{
    [SerializeField] UpgradeWindowUI upgradeWindow;
    [SerializeField] TMPro.TMP_Text description;
    [SerializeField] Button upgradeButton;
    [SerializeField] Sprite defaultSprite;
    [SerializeField] Sprite selectedSprite;
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
        for (int i = 0; i < images.Length; i++)
        {
            if (i < spellSet.upgrades.Count)
            {
                images[i].transform.parent.gameObject.SetActive(true);
                images[i].sprite = spellSet.upgrades[i].spell.spellIcon;
            }
            else
            {
                images[i].transform.parent.gameObject.SetActive(false);
            }

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

    public void SetActiveSprite(int upgradeIdx)
    {
        for (int i = 0; i < images.Length; i++)
        {
            images[i].transform.parent.GetComponent<Image>().sprite = i == upgradeIdx ? selectedSprite : defaultSprite;
        }
    }

}
