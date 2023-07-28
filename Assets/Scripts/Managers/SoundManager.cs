using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SoundManager : MonoBehaviour
{

    private const string PLAYERPREFS_SOUNDEFFECTS_VOLUME = "SoundEffectsVolume";

    public static SoundManager Instance { get; private set; }

    [SerializeField] private AudioClipsSO audioClipsSO;

    [Header("AudioSource")]
    [SerializeField] private AudioSource SoundAudioSource;
   
    private float volume;

    private void Awake()
    {

        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(this);

        }
        else
        {
            Destroy(gameObject);
        }

        volume = SoundAudioSource.volume;
        volume = PlayerPrefs.GetFloat(PLAYERPREFS_SOUNDEFFECTS_VOLUME, volume);

    }

    //-----------------------------------------------------------------------------------------------------------

    /// <summary>
    /// Sound selectio from an array of audioclips
    /// </summary>
    /// <param name="audioClipArray"></param>
    /// <param name="position"></param>
    /// <param name="volume"></param>
    private void PlayRandomSound(AudioClip[] audioClipArray)
    {
        PlaySound(audioClipArray[Random.Range(0, audioClipArray.Length)]);
    }

    //-----------------------------------------------------------------------------------------------------------

    /// <summary>
    /// Play audiclip
    /// </summary>
    /// <param name="audioClip"></param>
    /// <param name="position"></param>
    /// <param name="volumeMultiplier"></param>
    private void PlaySound(AudioClip audioClip)
    {
        SoundAudioSource.PlayOneShot(audioClip);
    }

    //-----------------------------------------------------------------------------------------------------------

    public void PlayMenuSound()
    {
        PlayRandomSound(audioClipsSO.ButtonsClick);
    }

    //-----------------------------------------------------------------------------------------------------------

    public void PlayPlayerReady()
    {
        PlayRandomSound(audioClipsSO.playerReadyCharacterSelect);
    }

    //-----------------------------------------------------------------------------------------------------------

    /// <summary>
    /// PlaySound when countdown starts
    /// </summary>
    public void PlayCountdownSound()
    {
        PlayRandomSound(audioClipsSO.CountDown);
    }

    //-----------------------------------------------------------------------------------------------------------
    /// <summary>
    /// Play sound when jumping
    /// </summary>
    /// <param name="position"></param>
    /// <param name="volume"></param>
    public void PlayJumpSound()
    {
        PlayRandomSound(audioClipsSO.playerJump);
    }

    //-----------------------------------------------------------------------------------------------------------

    /// <summary>
    /// Play audio when obstacle hits the player
    /// </summary>
    /// <param name="position"></param>
    /// <param name="volume"></param>
    public void PlayObstacleHittingPlayer()
    {
        PlayRandomSound(audioClipsSO.obstacleHittingPlayer);
    }

    //-----------------------------------------------------------------------------------------------------------


    /// <summary>
    /// Play sound when jumping
    /// </summary>
    /// <param name="position"></param>
    /// <param name="volume"></param>
    public void PlayStunnedSound()
    {
        PlayRandomSound(audioClipsSO.playerStunned);
    }

    //-----------------------------------------------------------------------------------------------------------

    public void PlayFallingSound()
    {
        PlayRandomSound(audioClipsSO.playerFalling);
    }

    //-----------------------------------------------------------------------------------------------------------

    /// <summary>
    /// Play audiclip for the steps
    /// </summary>
    /// <param name="position"></param>
    /// <param name="volume"></param>
    public void PlayFootstepsSound()
    {
        PlayRandomSound(audioClipsSO.footsteps);
    }

    //-----------------------------------------------------------------------------------------------------------

    /// <summary>
    /// Play audiclip for the steps
    /// </summary>
    /// <param name="position"></param>
    /// <param name="volume"></param>
    public void PlayAttackSound()
    {
        PlayRandomSound(audioClipsSO.playerPunch);
    }

    //-----------------------------------------------------------------------------------------------------------

    /// <summary>
    /// Play audiclip for the steps
    /// </summary>
    /// <param name="position"></param>
    /// <param name="volume"></param>
    public void PlayPlayerWinsLive()
    {
        PlayRandomSound(audioClipsSO.playerWinsLive);
    }

    //-----------------------------------------------------------------------------------------------------------

    /// <summary>
    /// Play audiclip for the steps
    /// </summary>
    /// <param name="position"></param>
    /// <param name="volume"></param>
    public void PlayPickUpPowerUp()
    {
        PlayRandomSound(audioClipsSO.playerPowerUp);
    }
    //-----------------------------------------------------------------------------------------------------------

    /// <summary>
    /// Change Music Volume
    /// </summary>
    public void ChangeVolume(float value)
    {
        SoundAudioSource.volume = value;

        PlayerPrefs.SetFloat(PLAYERPREFS_SOUNDEFFECTS_VOLUME, value);
        PlayerPrefs.Save();
    }

    /// <summary>
    /// Get the sound volume
    /// </summary>
    public float GetVolume()
    {
        return volume;
    }

}


