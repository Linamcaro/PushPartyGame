using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(AudioSource))]
public class MusicManager : MonoBehaviour
{

    private const string PLAYERPREFS_MUSICVOLUME = "MusicVolume";

    public static MusicManager Instance { get; private set; }

    [SerializeField] private AudioSource audioSource;

    [SerializeField] private AudioClip menuMusic;

    [SerializeField] private AudioClip gameMusic;

    [SerializeField] private AudioClip victoryMusic;

    [SerializeField] private AudioClip loserMusic;

    private float volume;


    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();

        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(this);

        }
        else
        {
            Destroy(gameObject);
        }

        volume = audioSource.volume;
        volume = PlayerPrefs.GetFloat(PLAYERPREFS_MUSICVOLUME, volume);

    }

    public void PlayMenuMusic(bool restart)
    {
        PlayTrack(menuMusic, true, false);
    }

    public void PlayGameMusic(bool restart)
    {
        PlayTrack(gameMusic, true, false);
    }

    public void PlayVictoryMusic(bool restart)
    {
        PlayTrack(victoryMusic, true, false);
    }

    public void PlayLoserMusic(bool restart)
    {
        PlayTrack(loserMusic, true, false);
    }


    private void PlayTrack(AudioClip clip, bool looping, bool restart)
    {
        if (audioSource.isPlaying)
        {
            //if we don want to restart the clip then return
            if (!restart && audioSource.clip == clip) { return; }
            audioSource.Stop();
        }

        audioSource.clip = clip;
        audioSource.loop = looping;
        audioSource.time = 0;
        audioSource.Play();

    }

    /// <summary>
    /// Change Music Volume
    /// </summary>
    public void ChangeVolume(float value)
    {
        audioSource.volume = value;

        PlayerPrefs.SetFloat(PLAYERPREFS_MUSICVOLUME, value);
        PlayerPrefs.Save();
    }

    public float GetVolume()
    {
        return volume;
    }

}
