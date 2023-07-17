using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SoundManager : MonoBehaviour
{
    private const string PLAYERPREFS_SOUNDEFFECTS_VOLUME = "SoundEffectsVolume";

    public static SoundManager Instance { get; private set; }

    [SerializeField] private AudioClipsSO audioClipsSO;

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

        volume = PlayerPrefs.GetFloat(PLAYERPREFS_SOUNDEFFECTS_VOLUME, volume);

    }

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

    /// <summary>
    /// Play audiclip
    /// </summary>
    /// <param name="audioClip"></param>
    /// <param name="position"></param>
    /// <param name="volumeMultiplier"></param>
    private void PlaySound(AudioClip audioClip, Vector3 position)
    {
        AudioSource.PlayClipAtPoint(audioClip, position, volumeMultiplier * volume);
    }


    /// <summary>
    /// Play audiclip for the steps
    /// </summary>
    /// <param name="position"></param>
    /// <param name="volume"></param>
    public void PlayFootstepsSound()
    {
        PlayRandomSound(audioClipsSO.footsteps);
    }

    /// <summary>
    /// Play sound when jumping
    /// </summary>
    /// <param name="position"></param>
    /// <param name="volume"></param>
    public void PlayJumpSound()
    {
        PlayRandomSound(audioClipsSO.playerJump);
    }

    /// <summary>
    /// Play audio when player slide
    /// </summary>
    /// <param name="position"></param>
    /// <param name="volume"></param>
    public void PlayerWinsLiveSound(Vector3 position, float volume)
    {
        PlayRandomSound(audioClipsSO.playerWinsLive);
    }

    /// <summary>
    /// Play audio when player attacks
    /// </summary>
    /// <param name="position"></param>
    /// <param name="volume"></param>
    public void PlayerPowerUpSound(Vector3 position, float volume)
    {
        PlayRandomSound(audioClipsSO.playerPowerUp);
    }

    /// <summary>
    /// Play audio when obstacle hits the player
    /// </summary>
    /// <param name="position"></param>
    /// <param name="volume"></param>
    public void PlayObstacleHittingPlayerSound()
    {
        PlayRandomSound(audioClipsSO.playerFalling);
    }

    /// <summary>
    /// PlaySound when countdown starts
    /// </summary>
    public void PlayCountdownSound()
    {
        PlayRandomSound(audioClipsSO.CountDown);
    }

    /// <summary>
    /// Play sound when jumping
    /// </summary>
    /// <param name="position"></param>
    /// <param name="volume"></param>
    public void PlayStunnedSound()
    {
        PlayRandomSound(audioClipsSO.playerStunned);
    }

    public void PlayerFallingSound()
    {
        PlayRandomSound(audioClipsSO.playerFalling);
    }

    public void PlayerMenuSound()
    {
        PlayRandomSound(audioClipsSO.ButtonsClick);
    }


    /// <summary>
    /// Change the sound volume
    /// </summary>
    public void ChangeVolume(float volumeChanged)
    {
        volume = volumeChanged;

        PlayerPrefs.SetFloat(PLAYERPREFS_SOUNDEFFECTS_VOLUME, volume);
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
