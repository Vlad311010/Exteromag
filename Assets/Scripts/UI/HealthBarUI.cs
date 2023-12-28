using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HealthBarUI : MonoBehaviour
{
    [SerializeField] Image filler;
    [SerializeField] TMP_Text text;

    void Awake()
    {
        GameEvents.current.onHealthChangePlayer += UpdateUI;
    }

    private void UpdateUI(int currentHp, int maxHp)
    {
        float percent = (float)currentHp / maxHp;
        filler.fillAmount = percent;
        text.text = string.Format("{0}/{1}", currentHp, maxHp);
    }
}
