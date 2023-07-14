
using UnityEngine;

public class MainMenuMusicStarter : MonoBehaviour
{
    // set whether theme should restart if already playing

    [SerializeField] bool m_Restart;

    void Start()
    {
        MusicManager.Instance.PlayMenuMusic(m_Restart);
    }
}

