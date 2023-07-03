using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "Sound Effects", menuName = "Sound/Sound Effects")]
public class AudioClipsSO : ScriptableObject
{ 
    public AudioClip[] playerSlide;
    public AudioClip[] playerFalling;
    public AudioClip[] playerAtack;
    public AudioClip[] footsteps;
    public AudioClip[] playerJump;
    public AudioClip[] obstacleHittingPlayer;
    public AudioClip[] CountDown;

}
