using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSounds : MonoBehaviour
{
    private static PlayerSounds _instance;
    public static PlayerSounds Instance;

    private float footstepTimer;
    private float footstepTimerMax = .1f;
    private float volume = 1f;


  

    private void Update() {
        FootStepSound();
        JumpSound();
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
               
                //SoundManager.Instance.PlayFootstepsSound(playerMovement.transform.position, volume);
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
            float volume = 1f;
            SoundManager.Instance.PlayJumpSound(PlayerMovement.PlayerMovementInstance.transform.position, volume);
           
        } 
    }

    /// <summary>
    /// Play Stunned sound when player jump
    /// </summary>
    private void PlayerStunnedSound()
    {
        if(PlayerMovement.PlayerMovementInstance.isStuned)
        {
            float volume = 1f;
            SoundManager.Instance.PlayStunnedSound(PlayerMovement.PlayerMovementInstance.transform.position, volume);
        }
    }

}
