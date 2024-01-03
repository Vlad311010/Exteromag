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
        
    }

    private void UpdateValue()
    {
        SettingsSO.settings[variableName] = slider.value / 100f;
    }

    
}
