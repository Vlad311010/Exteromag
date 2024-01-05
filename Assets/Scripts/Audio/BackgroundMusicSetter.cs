using UnityEngine;

public class BackgroundMusicSetter : MonoBehaviour
{
    [SerializeField] private AudioClip music;
    [SerializeField] private float musicVolume;
    [SerializeField] private GameObject musicPlayerPrefab; 
    
    void Start()
    {
        GameObject musicPlayerGO = GameObject.FindGameObjectWithTag("BackgroundMusicPlayer");
        if (musicPlayerGO != null)
            musicPlayerGO.GetComponent<BackgroundMusic>().ChangeMusic(music, musicVolume);
        else
        {
            Instantiate(musicPlayerPrefab).GetComponent<BackgroundMusic>().ChangeMusic(music, musicVolume);
        }
    }

}
