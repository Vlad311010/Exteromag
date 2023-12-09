using UnityEngine;
using UnityEngine.UI;

public class SpellCooldownUI : MonoBehaviour
{
    [SerializeField] int slotIdx;
    
    [SerializeField] Transform aim;
    [SerializeField] Vector3 offset;


    [SerializeField] Image image;
    [SerializeField] Image center;
    
    [SerializeField] Material positive;
    [SerializeField] Material negative;

    private void OnEnable()
    {
        GameEvents.current.onSpellCooldownValueChange += UpdateUI;
        GameEvents.current.onPlayersDeath += Disable;

    }

    private void OnDisable()
    {
        GameEvents.current.onSpellCooldownValueChange -= UpdateUI;
    }
    private void LateUpdate()
    {
        
        transform.position = aim.position + offset;
    }


    private void UpdateUI(float percent, int slotIdx)
    {
        if (this.slotIdx != slotIdx) return;

        image.fillAmount = percent;
        if (image.fillAmount < 1)
            center.material = negative;
        else
            center.material = positive;
    }

    private void Disable()
    {
        gameObject.SetActive(false);
    }
    
}
