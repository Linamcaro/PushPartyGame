
using UnityEngine;

public class MainMenuMusicStarter : MonoBehaviour
{
    // set whether theme should restart if already playing

    [SerializeField] bool restart;

    void Start()
    {
        MusicManager.Instance.PlayMenuMusic(restart);
    }
}

