using UnityEngine;
using UnityEngine.UI;
using System;
public class UpdateSettingsSO : MonoBehaviour
{
    [SerializeField] string variableName = "";

    Slider slider;
    
    void Start()
    {
        slider = GetComponent<Slider>();
        slider.onValueChanged.AddListener(delegate { UpdateValue(); });

        slider.value = SettingsSO.Get(variableName) * 100;
    }

    private void UpdateValue()
    {
        SettingsSO.Set(variableName, slider.value / 100f);
    }

    
}
