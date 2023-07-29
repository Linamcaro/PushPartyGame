
using UnityEngine;

public class MainMenuMusicStarter : MonoBehaviour
{
    // set whether theme should restart if already playing

    [SerializeField] bool restart;

    void Start()
    {
        MusicManager.Instance.PlayMenuMusic(restart);
    }


    public void PlayMenuSound()
    {
        SoundManager.Instance.PlayMenuSound();
    }

    public void PlayPlayerReadySound()
    { 
        SoundManager.Instance.PlayPlayerReady();
    }

}

