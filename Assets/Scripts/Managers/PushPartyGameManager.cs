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
    private NetworkVariable<State> state = new NetworkVariable<State>(State.WaitingToStart);


    private NetworkVariable<float> countdownTostartTimer = new NetworkVariable<float>(3f);
    private bool isLocalPlayerReady;

    //Store the player ID and if it is ready
    private Dictionary<ulong, bool> playerReadyDictionary;

    //Event for the game state changes
    public event EventHandler OnStateChanged;

    private void Awake()
    {
        _instance = this;
        Debug.Log("Is game playing false so return");
        playerReadyDictionary = new Dictionary<ulong, bool>();
    }


    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();

        state.OnValueChanged += State_OnValueChanged;

    }

    /// <summary>
    /// When the state value change fire the OnStateChanged event
    /// </summary>
    /// <param name="previousValue"></param>
    /// <param name="newValue"></param>
    private void State_OnValueChanged(State previousValue, State newValue)
    {
        OnStateChanged?.Invoke(this, EventArgs.Empty);
    }

    /// <summary>
    /// Check if players are ready
    /// </summary>
    private void OnPlayersReady()
    {
        Debug.Log("OnPlayersReady function called");
        if (state.Value == State.WaitingToStart)
        {   
            Debug.Log("state changed to waiting: " + state);
            isLocalPlayerReady = true;
            SetPlayerReadyServerRpc();
            
        }
    }

    /// <summary>
    /// Tell the server if players are ready 
    /// </summary>
    /// <param name="serverRpcParams"></param>
    [ServerRpc(RequireOwnership = false)]
    private void SetPlayerReadyServerRpc(ServerRpcParams serverRpcParams = default)
    { 
        playerReadyDictionary[serverRpcParams.Receive.SenderClientId] = true;
        Debug.Log(serverRpcParams.Receive.SenderClientId);

        bool allClientsReady = true;
        
       foreach(ulong clientId in NetworkManager.Singleton.ConnectedClientsIds)
        {
            //checki if it does not contains the key or is not ready
            if (!playerReadyDictionary.ContainsKey(clientId) || !playerReadyDictionary[clientId])
            {
                Debug.Log("SetPlayerReadyServerRPC function called");
                allClientsReady = false;
                break;
            }
        }

       //If all clients are ready start countdown;
       if(allClientsReady)
        {
            state.Value = State.CountdownToStart;
        }
        Debug.Log("allClientsReady: " + allClientsReady);
        
    }

    private void Update()
    {
        if (!IsServer) { return; }

        switch (state.Value)
        {
           
            case State.WaitingToStart:
                //Debug.Log("WaitingToStart");
                break;

            case State.CountdownToStart:
                countdownTostartTimer.Value -= Time.deltaTime;
                if (countdownTostartTimer.Value < 0f)
                {
                    state.Value = State.GamePlaying;
                }
                //Debug.Log("CountdownToStart" + countdownTostartTimer);
                break;

            case State.GamePlaying:
                /*int playerLives = PlayerRespawn.Instance.GetPlayerLives();
                if (playerLives  <= 0)
                {
                    state = State.GameOver;
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
        return state.Value == State.GamePlaying;
    }

    //Return true if the countdown to start is active
    public bool IsCountdownToStartActive()
    {
        return state.Value == State.CountdownToStart;
    }

    //Call the timer value
    public float GetCountdownToStartTimer()
    {
        return countdownTostartTimer.Value;
    }

    //Return true if the countdown to start is active
    public bool IsGameOver()
    {
        return state.Value == State.GameOver;
    }

    //Return true if player is ready
    public bool IsLocalPlayerReady()
    {
        Debug.Log("SetPlayerReadyServerRPC function called");
        return isLocalPlayerReady;
    }

  public void OnStartButtonPressed()
    {
        Debug.Log("OnStartButtonPressed function called");
        OnPlayersReady();
        
    }

}
