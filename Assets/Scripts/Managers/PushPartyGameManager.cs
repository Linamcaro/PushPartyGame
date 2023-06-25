using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;



public class PushPartyGameManager : MonoBehaviour
{
    private static PushPartyGameManager _instance;
    public static PushPartyGameManager Instance
    {
        get { return _instance; }
    }

    private enum State
    {
        WaitingToStart,
        CountdownToStart,
        GamePlaying,
        GameOver,
    }

    private State state;
    private float waitingTostartTimer = 5f;
    private float countdownTostartTimer = 5f;
    private float GamePlayingTimer = 10f;


    private void Awake()
    {
        _instance = this;
        state = State.WaitingToStart;
        Debug.Log("Is game playing false so return");
    }

    private void Update()
    {
        switch (state)
        {
            case State.WaitingToStart:
                waitingTostartTimer -= Time.deltaTime;
                if(waitingTostartTimer <0f)
                {
                    state = State.CountdownToStart;
                }
                Debug.Log("WaitingToStart");
                break;

            case State.CountdownToStart:
                countdownTostartTimer -= Time.deltaTime;
                if (countdownTostartTimer < 0f)
                {
                    state = State.GamePlaying;
                }
                Debug.Log("CountdownToStart");
                break;

            case State.GamePlaying:
                GamePlayingTimer -= Time.deltaTime;
                if (GamePlayingTimer < 0f)
                {
                    state = State.GameOver;
                }
                Debug.Log("GamePlaying");
                break;

            case State.GameOver:
                break;
        }
    }

    public bool IsGamePlaying()
    {
        return state == State.GamePlaying;
    }





   /* public bool IsCountdownToStartActive()
    {
        return state.Value == State.CountdownToStart;
    }


    public float GetCountdownToStartTimer()
    {
        return countdownToStartTimer.Value;
    }*/





}
