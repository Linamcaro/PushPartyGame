using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MusicManager : MonoBehaviour
{

    //[SerializeField] private Slider volumeSlider;
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
