
using System.Collections.Generic;
using UnityEngine;



public class MusicManager : MonoBehaviour
{

    [SerializeField] private List<AudioClip> audioClips;
    private const string PLAYERPREFS_MUSICVOLUME = "MusicVolume";


    private static MusicManager _instance;
    public static MusicManager Instance
    {
        get
        {
            return _instance;
        }
    }


    private AudioSource audioSource;
    private float volume;


    private void Awake()
    {
         
        _instance = this;

        audioSource = GetComponent<AudioSource>();

        volume = audioSource.volume;
        volume = PlayerPrefs.GetFloat(PLAYERPREFS_MUSICVOLUME, volume);

    }
    private void Start()
    {
        /*if (SceneManager.GetActiveScene().name == "MainScene")
        {
            PushPartyGameManager.Instance.OnStateChanged += PushPartyGameManager_OnStateChanged;
        }*/
    }

    /*private void PushPartyGameManager_OnStateChanged(object sender, EventArgs e)
    {
        if (!PushPartyGameManager.Instance.IsGameOver())
        {

            MusicManager.Instance.GamePlayingMusic();

        }
        else
        {

            if (PlayerSpawn.Instance.GetPlayerLives() > 0)
            {

                MusicManager.Instance.WinnerMusic();

            }
            else
            {
                MusicManager.Instance.GameOverMusic();

            }
        }
    }

    public void GamePlayingMusic()
    {
        AudioSource.PlayClipAtPoint(audioClips[0], Vector3.zero);
    }
    public void GameOverMusic()
    {
        AudioSource.PlayClipAtPoint(audioClips[2], Vector3.zero);
    }
    public void WinnerMusic()
    {
        AudioSource.PlayClipAtPoint(audioClips[1], Vector3.zero);
    }*/


    /// <summary>
    /// Change Music Volume
    /// </summary>
    public void ChangeVolume()
    {
       audioSource.volume = volume;

        PlayerPrefs.SetFloat(PLAYERPREFS_MUSICVOLUME, volume);
        PlayerPrefs.Save();
    }

    public float GetVolume()
    {
        return volume;
    }

}
