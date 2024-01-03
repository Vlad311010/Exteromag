using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class OptionsSliderUI : MonoBehaviour
{
    [SerializeField] Slider slider;
    [SerializeField] TMP_Text text;

    private void Awake()
    {
        slider.onValueChanged.AddListener(delegate { UpdateText(); });
    }

    private void UpdateText()
    {
        text.text = slider.value + "%";
    }
}
