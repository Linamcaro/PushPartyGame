using System;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;



public class PushPartyGameManager : NetworkBehaviour
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

    //Handle game states
    private State state;


    private float countdownTostartTimer = 3f;
    private bool isLocalPlayerReady;

    private Dictionary<ulong, bool> playerReadyDictionary;

    //Event for the game state changes
    public event EventHandler OnStateChanged;

    private void Awake()
    {
        _instance = this;
        state = State.WaitingToStart;
        Debug.Log("Is game playing false so return");
        playerReadyDictionary = new Dictionary<ulong, bool>();
    }
    
    /// <summary>
    /// Check if players are ready
    /// </summary>
   private void OnPlayersReady()
    {
        Debug.Log("OnPlayersReady function called");
        if (state == State.WaitingToStart)
        {   
            Debug.Log("state changed: " + state);
            isLocalPlayerReady = true;
            SetPlayerReadyServerRPC();
            
        }
    }

    /// <summary>
    /// Tell the server if players are ready 
    /// </summary>
    /// <param name="serverRpcParams"></param>
    [ServerRpc(RequireOwnership = false)]
    private void SetPlayerReadyServerRPC(ServerRpcParams serverRpcParams = default)
    {
        playerReadyDictionary[serverRpcParams.Receive.SenderClientId] = true;
        Debug.Log(serverRpcParams.Receive.SenderClientId);
        bool allClientsReady = true;
        foreach(ulong clientId in NetworkManager.Singleton.ConnectedClientsIds)
        {
            if(!playerReadyDictionary.ContainsKey(clientId) || !playerReadyDictionary[clientId])
            {
                allClientsReady = false;
                break;
            }
        }

        Debug.Log("allClientsReady: " + allClientsReady);
        
    }

    private void Update()
    {

        //SetPlayerReadyServerRPC();
        switch (state)
        {
            case State.WaitingToStart:
                //Debug.Log("WaitingToStart");
                break;

            case State.CountdownToStart:
                countdownTostartTimer -= Time.deltaTime;
                if (countdownTostartTimer < 0f)
                {
                    state = State.GamePlaying;
                    OnStateChanged?.Invoke(this, EventArgs.Empty);
                }
                //Debug.Log("CountdownToStart" + countdownTostartTimer);
                break;

            case State.GamePlaying:
                /*int playerLives = PlayerRespawn.Instance.GetPlayerLives();
                if (playerLives  <= 0)
                {
                    state = State.GameOver;
                    OnStateChanged?.Invoke(this, EventArgs.Empty);
                }*/
                Debug.Log("GamePlaying");
                break;

            case State.GameOver:
                break;
        }
    }

    //Return true if game is playing
    public bool IsGamePlaying()
    {
        return state == State.GamePlaying;
    }

    //Return true if the countdown to start is active
    public bool IsCountdownToStartActive()
    {
        return state == State.CountdownToStart;
    }

    //Call the timer value
    public float GetCountdownToStartTimer()
    {
        return countdownTostartTimer;
    }

    //Return true if the countdown to start is active
    public bool IsGameOver()
    {
        return state == State.GameOver;
    }

    //Return true if player is ready
    public bool IsLocalPlayerReady()
    {
            return isLocalPlayerReady;
    }

  public void OnStartButtonPressed()
    {
        Debug.Log("OnStartButtonPressed function called");
        OnPlayersReady();
        
    }

}
