public class SFXPlayer : SoundPlayer
{
    protected override void SetDefaultSoundSettings()
    {
        audio.minDistance = 5;
        audio.maxDistance = 100;
        audio.spatialBlend = 0.7f;
    }

    public override void SetSoundVolume()
    {
        audio.volume = defaultVolume * SettingsSO.Get("sfxVolume");
    }
}


