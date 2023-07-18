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
    private float volumeMultiplier = 2;

    private Vector3 cameraPosition;

    //-----------------------------------------------------------------------------------------------------------

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

        cameraPosition = Camera.main.transform.position;

    }

    //-----------------------------------------------------------------------------------------------------------

    /// <summary>
    /// Sound selectio from an array of audioclips
    /// </summary>
    /// <param name="audioClipArray"></param>
    /// <param name="position"></param>
    /// <param name="volume"></param>
    private void PlayRandomSound(AudioClip[] audioClipArray, Vector3 position)
    {
        PlaySound(audioClipArray[Random.Range(0, audioClipArray.Length)], position);
    }

    //-----------------------------------------------------------------------------------------------------------

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

    //-----------------------------------------------------------------------------------------------------------

    /// <summary>
    /// Play audiclip for the steps
    /// </summary>
    /// <param name="position"></param>
    /// <param name="volume"></param>
    public void PlayFootstepsSound(Vector3 position)
    {
        PlayRandomSound(audioClipsSO.footsteps, position);
    }

    //-----------------------------------------------------------------------------------------------------------

    /// <summary>
    /// Play sound when jumping
    /// </summary>
    /// <param name="position"></param>
    /// <param name="volume"></param>
    public void PlayJumpSound(Vector3 position)
    {
        PlayRandomSound(audioClipsSO.playerJump, position);
    }


    //-----------------------------------------------------------------------------------------------------------

    /// <summary>
    /// Play audio when player attacks
    /// </summary>
    /// <param name="position"></param>
    /// <param name="volume"></param>
    public void PlayPowerUpSound(Vector3 position)
    {
        PlayRandomSound(audioClipsSO.playerPowerUp, position);
    }

    //-----------------------------------------------------------------------------------------------------------

    /// <summary>
    /// Play audio when obstacle hits the player
    /// </summary>
    /// <param name="position"></param>
    /// <param name="volume"></param>
    public void PlayObstacleHittingPlayerSound(Vector3 position)
    {
        PlayRandomSound(audioClipsSO.obstacleHittingPlayer, position);
    }

    //-----------------------------------------------------------------------------------------------------------

    /// <summary>
    /// PlaySound when countdown starts
    /// </summary>
    public void PlayCountdownSound()
    {
        PlayRandomSound(audioClipsSO.CountDown, cameraPosition);
    }

    //-----------------------------------------------------------------------------------------------------------

    /// <summary>
    /// Play sound when jumping
    /// </summary>
    /// <param name="position"></param>
    /// <param name="volume"></param>
    public void PlayStunnedSound(Vector3 position)
    {
        PlayRandomSound(audioClipsSO.playerStunned,position);
    }

    //-----------------------------------------------------------------------------------------------------------

    public void PlayFallingSound()
    {
        PlayRandomSound(audioClipsSO.playerFalling, cameraPosition);
    }

    //-----------------------------------------------------------------------------------------------------------

    public void PlayMenuSound()
    {
        PlayRandomSound(audioClipsSO.ButtonsClick, cameraPosition);
    }

    //-----------------------------------------------------------------------------------------------------------

    /// <summary>
    /// Change the sound volume
    /// </summary>
    public void ChangeVolume(float volumeChanged)
    {
        volume = volumeChanged;

        PlayerPrefs.SetFloat(PLAYERPREFS_SOUNDEFFECTS_VOLUME, volume);
        PlayerPrefs.Save();
    }

    //-----------------------------------------------------------------------------------------------------------

    /// <summary>
    /// Get the sound volume
    /// </summary>
    public float GetVolume()
    {
        return volume;
    }


}
