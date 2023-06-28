using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSounds : MonoBehaviour
{

    private PlayerMovement playerMovement;
    private float footstepTimer;
    private float footstepTimerMax = .1f;
    private float volume = 1f;


    private void Awake() {
        playerMovement = GetComponent<PlayerMovement>();
    }

    private void Update() {
        FootStepSound();
        JumpSound();
        SlideSound();
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

            if (playerMovement.IsWalking())
            {
               
                SoundManager.Instance.PlayFootstepsSound(playerMovement.transform.position, volume);
            }
        }
    }

    /// <summary>
    /// Play Jump sound when player jump
    /// </summary>
    private void JumpSound()
    {
        if (playerMovement.IsJumping())
        {
            SoundManager.Instance.PlayJumpSound(playerMovement.transform.position, volume);
           
        } 
    }

    /// <summary>
    /// Play Slide Soubd when player Slide
    /// </summary>
    private void SlideSound()
    {
        if (playerMovement.IsSliding())
        {
            SoundManager.Instance.PlaySlideSound(playerMovement.transform.position, volume);

        }
    }



}
