using System;
using System.Collections;
using UnityEngine;

public class MainSceneMusic : MonoBehaviour
{

    // set whether theme should restart if already playing

    [SerializeField] bool restart;

    private bool canPlaySound = true;


    private void Start()
    {

        PushPartyGameManager.Instance.OnStateChanged += PushPartyGameManager_OnStateChanged;

        if (!PushPartyGameManager.Instance.IsGamePlaying())
        {
            MusicManager.Instance.PlayGameMusic(restart);
        }

    }

    private void PushPartyGameManager_OnStateChanged(object sender, EventArgs e)
    {
        if (PushPartyGameManager.Instance.IsGameOver())
        {

            if (PlayerSpawn.Instance.GetPlayerLives() > 0)
            {

                MusicManager.Instance.PlayVictoryMusic(restart);

            }
            else
            {
                MusicManager.Instance.PlayLoserMusic(restart);

            }
        }
    }


        /// <summary>
        /// Play sound when a menu button is clicked
        /// </summary>
    public void PlayMenuSound()
    {
        SoundManager.Instance.PlayMenuSound();
    }

    /// <summary>
    /// Play Sound when player ready button is clicked
    /// </summary>
    public void PlayPlayerReadySound()
    {
        SoundManager.Instance.PlayPlayerReady();
    }

}