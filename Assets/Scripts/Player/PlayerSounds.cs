using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSounds : MonoBehaviour
{
    private static PlayerSounds _instance;
    public static PlayerSounds Instance;

    private float footstepTimer;
    private float footstepTimerMax = .1f;
    private float volume = 0f;


    public event EventHandler OnPlayerFell;


    private void Start()
    {
        PlayerRespawn.Instance.OnPlayerFell += PlayerRespawm_OnPlayerFell;
    }

  
    private void Update() {
        FootStepSound();
        JumpSound();
        PlayerStunnedSound();
    }

    private void PlayerRespawm_OnPlayerFell(object sender, EventArgs e)
    {
        SoundManager.Instance.PlayerFallingSound(gameObject.transform.position, volume);
    }


    /// <summary>
    /// Play footstep sound if player is walking
    /// </summary>
    private void FootStepSound()
    {
        footstepTimer -= Time.deltaTime;
        if (footstepTimer < 0f)
        {
            footstepTimer = footstepTimerMax;

            if (PlayerMovement.PlayerMovementInstance.getVelocity() > 0f)
            {
               
                SoundManager.Instance.PlayFootstepsSound(PlayerMovement.PlayerMovementInstance.transform.position, volume);
            }
        }
    }

    /// <summary>
    /// Play Jump sound when player jump
    /// </summary>
    private void JumpSound()
    {
        if (PlayerMovement.PlayerMovementInstance.isJumping)
        {
            SoundManager.Instance.PlayJumpSound(PlayerMovement.PlayerMovementInstance.transform.position, volume);
           
        } 
    }

    /// <summary>
    /// Play stunned sound when player jump
    /// </summary>
    private void PlayerStunnedSound()
    {
        if(PlayerMovement.PlayerMovementInstance.isStuned)
        {
            SoundManager.Instance.PlayStunnedSound(gameObject.transform.position, volume);
        }
    }

    




    /*IEnumerator SoundDelay()
    {
        
    }*/

}
