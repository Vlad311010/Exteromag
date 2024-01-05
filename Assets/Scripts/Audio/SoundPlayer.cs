using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public abstract class SoundPlayer : MonoBehaviour
{
    public AudioClip Clip { get => audio.clip; }

    protected AudioSource audio;

    protected float defaultVolume;

    protected virtual void Awake()
    {
        audio = GetComponent<AudioSource>();
        defaultVolume = audio.volume;

        // GameEvents.current.onSettingsChange += SetSoundVolume;
        SetDefaultSoundSettings();
        SetSoundVolume();
    }

    protected abstract void SetDefaultSoundSettings();
    public abstract void SetSoundVolume();
    
    public void Play(AudioClip clip)
    {
        audio.clip = clip;
        audio.Play();
    }

    public void Play(AudioClip clip, float radnomPitchIncreaseValue)
    {
        audio.pitch = 1f + Random.Range(0, radnomPitchIncreaseValue);
        audio.clip = clip;
        audio.Play();
    }

    protected virtual void OnDisable()
    {
        // GameEvents.current.onSettingsChange -= SetSoundVolume;
    }

}
