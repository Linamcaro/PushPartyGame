using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class SoundManager : MonoBehaviour
{
    private const string PLAYERPREFS_SOUNDEFFECTS_VOLUME = "SoundEffectsVolume";


    private static SoundManager _instance;
    [SerializeField] private Slider volumeSlider;
    private float volume;
    public static SoundManager Instance
    {
        get
        {
            return _instance;
        }
    }

    [SerializeField] private AudioClipsSO audioClipsSO;



    private void Awake()
    {
        _instance = this;

        volume = PlayerPrefs.GetFloat(PLAYERPREFS_SOUNDEFFECTS_VOLUME, 0f);

    }

    /// <summary>
    /// Sound selectio from an array of audioclips
    /// </summary>
    /// <param name="audioClipArray"></param>
    /// <param name="position"></param>
    /// <param name="volume"></param>
    private void PlayRandomSound(AudioClip[] audioClipArray, Vector3 position, float volume)
    {
        PlaySound(audioClipArray[Random.Range(0, audioClipArray.Length)], position, volume);
    }

    /// <summary>
    /// Play audiclip
    /// </summary>
    /// <param name="audioClip"></param>
    /// <param name="position"></param>
    /// <param name="volumeMultiplier"></param>
    private void PlaySound(AudioClip audioClip, Vector3 position, float volumeMultiplier = 1f)
    {
        AudioSource.PlayClipAtPoint(audioClip, position, volumeMultiplier * volume);
        Debug.Log("AudioClip seleccionado: " + audioClip.name);
        Debug.Log("Volumen del sonido: " + (volumeMultiplier * volume));
        Debug.Log("Volumen del AudioSource: " + volume);
    }




    /// <summary>
    /// Play audiclip for the steps
    /// </summary>
    /// <param name="position"></param>
    /// <param name="volume"></param>
    public void PlayFootstepsSound(Vector3 position)
    {
        PlayRandomSound(audioClipsSO.footsteps, position, volume);
    }

    /// <summary>
    /// Play sound when jumping
    /// </summary>
    /// <param name="position"></param>
    /// <param name="volume"></param>
    public void PlayJumpSound(Vector3 position)
    {
        PlayRandomSound(audioClipsSO.playerJump,position, volume);
    }

    /// <summary>
    /// Play audio when player slide
    /// </summary>
    /// <param name="position"></param>
    /// <param name="volume"></param>
    public void PlayerWinsLiveSound(Vector3 position)
    {
        PlayRandomSound(audioClipsSO.playerWinsLive, position, volume);
    }

    /// <summary>
    /// Play audio when player attacks
    /// </summary>
    /// <param name="position"></param>
    /// <param name="volume"></param>
    public void PlayerPowerUpSound(Vector3 position)
    {
        PlayRandomSound(audioClipsSO.playerPowerUp, position, volume);
    }

    /// <summary>
    /// Play audio when obstacle hits the player
    /// </summary>
    /// <param name="position"></param>
    /// <param name="volume"></param>
    public void PlayObstacleHittingPlayerSound(Vector3 position)
    {
        PlayRandomSound(audioClipsSO.playerFalling, position, volume);
    }

    /// <summary>
    /// PlaySound when countdown starts
    /// </summary>
    public void PlayCountdownSound()
    {
        PlayRandomSound(audioClipsSO.CountDown, Vector3.zero, volume);
    }

    /// <summary>
    /// Play sound when jumping
    /// </summary>
    /// <param name="position"></param>
    /// <param name="volume"></param>
    public void PlayStunnedSound(Vector3 position)
    {
        PlayRandomSound(audioClipsSO.playerStunned, position, volume);
    }

    public void PlayerFallingSound(Vector3 position)
    {
        PlayRandomSound(audioClipsSO.playerFalling, position, volume);
    }

    public void PlayerMenuSound()
    {
        PlayRandomSound(audioClipsSO.ButtonsClick, Vector3.zero);
    }


    /// <summary>
    /// Change the sound volume
    /// </summary>
    public void ChangeVolume(float volumeChanged)
    {
        volume = volumeSlider.value;

        PlayerPrefs.SetFloat(PLAYERPREFS_SOUNDEFFECTS_VOLUME, volume);
       
        PlayerPrefs.Save();
    }

    public float GetVolume()
    {
        return volume;
    }


}
