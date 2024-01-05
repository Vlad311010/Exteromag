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

    private static Dictionary<string, float> settings;

    public static float Get(string key)
    {
        float value = 1f;

        if (settings == null || settings.TryGetValue(key, out value))
            return value;
        else
            return 1f;
    }

    public static void Set(string key, float value)
    {
        settings[key] = value;
        GameEvents.current.SettingsChange();

        foreach (var sound  in GameObject.FindObjectsOfType<SoundPlayer>())
        {
            sound.SetSoundVolume();
        }
        
    }

    private void Awake()
    {
        if (settings == null)
        {
            settings = new Dictionary<string, float>();
            settings.Add("sfxVolume", 1);
            settings.Add("musicVolume", 1);
        }
        GameEvents.current.SettingsChange();
    }


}
