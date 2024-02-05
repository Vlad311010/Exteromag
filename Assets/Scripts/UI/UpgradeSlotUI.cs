using System.Collections.Generic;
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
            // gameObject.SetActive(false);
            for (int i = 0; i < images.Length; i++)
            {
                images[i].transform.parent.gameObject.SetActive(false);
            }
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
    

    private string SetTextColor(string text, string color)
    {
        return string.Format("<color={0}>{1}</color>", color, text);
    }

    private List<string> FormatDescription(string text)
    {
        List<string> bulletPoints = new List<string>();
        foreach (string t in text.Split(','))
        {
            if (t[0] == '-')
            {
                bulletPoints.Add(SetTextColor(t.Substring(1), "#FF0000"));
            }
            else
            {
                bulletPoints.Add(SetTextColor(t, "#005500"));
            }
        }
        return bulletPoints;
    }

    public void ShowUpgradeDescription(int idx)
    {
        int selectedTranslationIndex = spellSet.upgrades[idx].GetDescriptionTranslationIndex(Translator.selectedLangage);
        List<string> bulletPoints = FormatDescription(spellSet.upgrades[idx].translation[selectedTranslationIndex].text);
        description.text = "� " + string.Join("\n� ", bulletPoints);
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
            if (i == upgradeIdx)
            {
                images[i].transform.parent.GetComponent<Image>().sprite = selectedSprite;
                images[i].GetComponent<Image>().color = Color.white;
            }
            else
            {
                images[i].transform.parent.GetComponent<Image>().sprite = defaultSprite;
                images[i].GetComponent<Image>().color = Color.gray;
            }
        }
    }

}
