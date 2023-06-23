using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviour
{


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
    private float volume = .3f;


    private void Awake()
    {
        _instance = this;

        audioSource = GetComponent<AudioSource>();

        volume = PlayerPrefs.GetFloat(PLAYERPREFS_MUSICVOLUME, .3f);
        audioSource.volume = volume;
    }

    /// <summary>
    /// Change Music Volume
    /// </summary>
    public void ChangeVolume()
    {
        volume += .1f;
        if (volume > 1f)
        {
            volume = 0f;
        }
        audioSource.volume = volume;

        PlayerPrefs.SetFloat(PLAYERPREFS_MUSICVOLUME, volume);
        PlayerPrefs.Save();
    }

    public float GetVolume()
    {
        return volume;
    }

}
