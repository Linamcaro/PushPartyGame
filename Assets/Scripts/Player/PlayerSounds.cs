using System;
using System.Collections;
using UnityEngine;

public class PlayerSounds : MonoBehaviour
{
 
        private bool canPlaySound = true;



    private void Start()
    {

       PlayerRespawn.Instance.OnPlayerFell += PlayerSounds_OnPlayerFell;
       PlayerMovement.PlayerMovementInstance.OnPlayerJump += PlayerSounds_OnPlayerJump;
       PlayerMovement.PlayerMovementInstance.OnPlayerStunned += PlayerSounds_OnPlayerStunned;
       PlayerMovement.PlayerMovementInstance.OnPlayerRunning += PlayerSounds_OnPlayerRunning;
       PlayerMovement.PlayerMovementInstance.OnPlayerAttack1 += PlayerSounds_OnPlayeAttack1;
       PlayerMovement.PlayerMovementInstance.OnPlayerHit += PlayerSounds_OnPlayerHit;
       PlayerSpawn.Instance.OnLivesChanged += PlayerSounds_OnLivesChanged;
       PlayerMovement.PlayerMovementInstance.OnPickUpPowerUp += PlayerSounds_OnPickUpPowerUp;
       PlayerSpawn.Instance.OnPickUpPowerUp += PlayerSounds_OnPickUpPowerUp;

    }

    private void PlayerSounds_OnPickUpPowerUp(object sender, EventArgs e)
    {
        if (canPlaySound)
        {
            SoundManager.Instance.PlayPickUpPowerUp();
            StartCoroutine(PlaySound());
        }
        
    }

    private void PlayerSounds_OnLivesChanged(object sender, EventArgs e)
    {
        if (canPlaySound)
        {
            SoundManager.Instance.PlayPlayerWinsLive();
            StartCoroutine(PlaySound());
        }
    }

    private void PlayerSounds_OnPlayeAttack1(object sender, EventArgs e)
    {
        if (canPlaySound)
        {
            SoundManager.Instance.PlayAttackSound();
            StartCoroutine(PlaySound());
        }
    }

    //-----------------------------------------------------------------------------------------------------------
    private void PlayerSounds_OnPlayerHit(object sender, EventArgs e)
    {
        if (canPlaySound) 
        {
            SoundManager.Instance.PlayObstacleHittingPlayer();
            StartCoroutine(PlaySound());
        }
    }

    //-----------------------------------------------------------------------------------------------------------

    private void PlayerSounds_OnPlayerStunned(object sender, EventArgs e)
    {
        if (canPlaySound)
        {
            SoundManager.Instance.PlayStunnedSound();
            StartCoroutine(PlaySound());
        }
    }

    //-----------------------------------------------------------------------------------------------------------

    private void PlayerSounds_OnPlayerFell(object sender, EventArgs e)
    {
       if(canPlaySound)
       {
            SoundManager.Instance.PlayFallingSound();
            StartCoroutine(PlaySound());
       }
    }

    //-----------------------------------------------------------------------------------------------------------

    /// <summary>
    /// Plau jump sound
    /// </summary>
    private void PlayerSounds_OnPlayerJump(object sender, EventArgs e)
    {
        if (canPlaySound)
        {
            SoundManager.Instance.PlayJumpSound();
            StartCoroutine(PlaySound());
        }
    }

    //-----------------------------------------------------------------------------------------------------------

    /// <summary>
    /// Play footstep sound if player is walking
    /// </summary>

    private void PlayerSounds_OnPlayerRunning(object sender, EventArgs e)
    {
        if (canPlaySound)
        {
            SoundManager.Instance.PlayFootstepsSound();
            StartCoroutine(PlaySound());
        }
    }

    //-----------------------------------------------------------------------------------------------------------

    IEnumerator PlaySound()
    {
       canPlaySound = false;
       yield return new WaitForSeconds(0.2f);
       canPlaySound = true;
    }
}
