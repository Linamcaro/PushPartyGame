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

    }


    private void Update() 
    {
       FootStepSound();
       
    }

    //-----------------------------------------------------------------------------------------------------------

    private void PlayerSounds_OnPlayerStunned(object sender, EventArgs e)
    {
        if (!canPlaySound)
        {
            return;
        }

        SoundManager.Instance.PlayStunnedSound();
        StartCoroutine(PlaySound());
    }

    //-----------------------------------------------------------------------------------------------------------

    private void PlayerSounds_OnPlayerFell(object sender, EventArgs e)
    {
       if(!canPlaySound)
       {
          return;
       }

       SoundManager.Instance.PlayFallingSound();
    }

    //-----------------------------------------------------------------------------------------------------------

    /// <summary>
    /// Plau jump sound
    /// </summary>
    private void PlayerSounds_OnPlayerJump(object sender, EventArgs e)
    {
        if (canPlaySound)
        {
            return;
        }

        SoundManager.Instance.PlayJumpSound();
        StartCoroutine(PlaySound());
    }

    //-----------------------------------------------------------------------------------------------------------

    /// <summary>
    /// Play footstep sound if player is walking
    /// </summary>
    private void FootStepSound()
    {
        if (!PlayerMovement.PlayerMovementInstance.isRunning|| !canPlaySound)
        {
            return;
        }

        SoundManager.Instance.PlayFootstepsSound();
        StartCoroutine(PlaySound());
    }

       IEnumerator PlaySound()
       {
           canPlaySound = false;
           yield return new WaitForSeconds(0.15f);
           canPlaySound = true;
       }
}
