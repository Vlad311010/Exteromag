
public class MusicPlayer : SoundPlayer
{
    protected override void SetDefaultSoundSettings()
    {
        audio.loop = true;
        audio.spatialBlend = 0.0f;
    }

    public override void SetSoundVolume()
    {
        audio.volume = defaultVolume * SettingsSO.Get("musicVolume");
    }
}
