using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSounds : MonoBehaviour
{

    private float footstepTimer;
    private float footstepTimerMax = 0.5f;
    private float volume = 1f;

    private bool canPlaySound = true;


    private void Start()
    {
       PlayerRespawn.Instance.OnPlayerFell += PlayerRespawm_OnPlayerFell;
    }

    private void Update() 
    {
        FootStepSound();
        PlayerJumpSound();
        PlayerStunnedSound();
    }

    private void PlayerRespawm_OnPlayerFell(object sender, EventArgs e)
    {
        if (canPlaySound)
        {
            SoundManager.Instance.PlayerFallingSound(gameObject.transform.position, volume);
        }
    }


    /// <summary>
    /// Play footstep sound if player is walking
    /// </summary>
    private void FootStepSound()
    {


        if (PlayerMovement.PlayerMovementInstance.getVelocity() > 0f && canPlaySound)
        {

            SoundManager.Instance.PlayFootstepsSound(PlayerMovement.PlayerMovementInstance.transform.position, volume);
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
            SoundManager.Instance.PlayJumpSound(PlayerMovement.PlayerMovementInstance.transform.position, volume);
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
            SoundManager.Instance.PlayStunnedSound(gameObject.transform.position, volume);
            StartCoroutine(PlaySound());
        }
    }

    IEnumerator PlaySound()
    {
        canPlaySound = false;
        yield return new WaitForSeconds(1f);
        canPlaySound = false;
    }
}
