using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSounds : MonoBehaviour
{

    private float footstepTimer;
    private float footstepTimerMax = 0.5f;

    [Header("AudioSource")]
    [SerializeField] private AudioSource sfxAudioSource;

    private bool canPlaySound = true;

    private Vector3 position;

    //-----------------------------------------------------------------------------------------------------------

    private void Start()
    {
       position = transform.position;

       sfxAudioSource.volume = SoundManager.Instance.GetVolume();
       PlayerRespawn.Instance.OnPlayerFell += PlayerRespawm_OnPlayerFell;

    }

    private void Update() 
    {
        FootStepSound();
        PlayerJumpSound();
        PlayerStunnedSound();
    }
    
    //-----------------------------------------------------------------------------------------------------------

    private void PlayerRespawm_OnPlayerFell(object sender, EventArgs e)
    {
        if (canPlaySound)
        {
            SoundManager.Instance.PlayFallingSound();
        }
    }

    //-----------------------------------------------------------------------------------------------------------

    /// <summary>
    /// Play footstep sound if player is walking
    /// </summary>
    private void FootStepSound()
    {
        if (PlayerMovement.PlayerMovementInstance.getVelocity() > 0f && canPlaySound)
        {

            SoundManager.Instance.PlayFootstepsSound(position);
            StartCoroutine(PlaySound());
        }
    }

    /// <summary>
    /// Play Jump sound when player jump
    /// </summary>
    private void PlayerJumpSound()
    {
        if (PlayerMovement.PlayerMovementInstance.isJumping && canPlaySound)
        {
            SoundManager.Instance.PlayJumpSound(position);
            StartCoroutine(PlaySound());
        } 
    }

    /// <summary>
    /// Play stunned sound when player jump
    /// </summary>
    private void PlayerStunnedSound()
    { 
        if(PlayerMovement.PlayerMovementInstance.isStuned && canPlaySound)
        {
            SoundManager.Instance.PlayStunnedSound(position);
            StartCoroutine(PlaySound());
        }
    }

    IEnumerator PlaySound()
    {
        canPlaySound = false;
        yield return new WaitForSeconds(0.15f);
        canPlaySound = false;
    }
}
