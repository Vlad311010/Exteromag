using UnityEngine;

public class BackgroundMusic : MusicPlayer
{
    protected override void Awake()
    {
        base.Awake();
        DontDestroyOnLoad(gameObject);

        GameObject[] objs = GameObject.FindGameObjectsWithTag("BackgroundMusicPlayer");

        if (objs.Length > 1)
        {
            Destroy(this.gameObject);
            return;
        }

    }

    public void ChangeMusic(AudioClip clip, float volume = 1)
    {
        if (clip == Clip) return;

        SetAudioSourceVolume(volume);
        Play(clip);
    }


}
