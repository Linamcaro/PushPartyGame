using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "Sound Effects", menuName = "Sound/Sound Effects")]
public class AudioClipsSO : ScriptableObject
{
    //player
    public AudioClip[] playerWinsLive;
    public AudioClip[] playerFalling;
    public AudioClip[] playerPowerUp;
    public AudioClip[] footsteps;
    public AudioClip[] playerJump;
    public AudioClip[] playerStunned;

    //obstacle
    public AudioClip[] obstacleHittingPlayer;

    //UI
    public AudioClip[] CountDown;
    public AudioClip[] ButtonsClick;
    public AudioClip[] playerReadyCharacterSelect;
    public AudioClip[] playerWins;
    public AudioClip[] playerLose;
}
