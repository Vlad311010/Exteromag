using System.Collections.Generic;
using UnityEngine;

public class SettingsSO : ScriptableObject
{
    /*public static SettingsSO current;
    private void Awake()
    {
        current = this;
    }*/

    // public static float sfxVolume = 1;
    // public static float musicVolume = 1;

    public static Dictionary<string, float> settings;

    private void Awake()
    {
        if (settings == null)
        {
            settings = new Dictionary<string, float>();
            settings.Add("sfxVolume", 1);
            settings.Add("musicVolume", 1);
        }
    }


}
