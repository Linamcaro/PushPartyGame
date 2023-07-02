using System;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;

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


    //Handle game states on the network
    private NetworkVariable<State> state = new NetworkVariable<State>(State.WaitingToStart);
    //Event for the game state changes
    public event EventHandler OnStateChanged;

    //CountDown Tiime
    private NetworkVariable<float> countdownTostartTimer = new NetworkVariable<float>(3f);

    //Store the player ID and if it is ready
    private Dictionary<ulong, bool> playerReadyDictionary;
    //Store the player ID and if it died
    private Dictionary<ulong, bool> playerDiedDictionary;

    private bool isLocalPlayerReady;

    [SerializeField] private Transform playerPrefab;


    private void Awake()
    {
        _instance = this;
        Debug.Log("Is game playing false so return");
        playerReadyDictionary = new Dictionary<ulong, bool>();
    }


    public override void OnNetworkSpawn()
    {
        state.OnValueChanged += State_OnValueChanged;

        if (IsServer)
        {
            NetworkManager.Singleton.SceneManager.OnLoadEventCompleted += SceneManager_OnLoadEventCompleted;
        }

    }

    /// <summary>
    /// Spawn the players prefab
    /// </summary>
    /// <param name="sceneName"></param>
    /// <param name="loadSceneMode"></param>
    /// <param name="clientsCompleted"></param>
    /// <param name="clientsTimedOut"></param>
    private void SceneManager_OnLoadEventCompleted(string sceneName, LoadSceneMode loadSceneMode, List<ulong> clientsCompleted, List<ulong> clientsTimedOut)
    {
        foreach (ulong clientId in NetworkManager.Singleton.ConnectedClientsIds)
        {
            Transform playerTransform = Instantiate(playerPrefab);
            playerTransform.GetComponent<NetworkObject>().SpawnAsPlayerObject(clientId, true);
        }
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

        foreach (ulong clientId in NetworkManager.Singleton.ConnectedClientsIds)
        {
            //check  if it does not contains the key or is not ready
            if (!playerReadyDictionary.ContainsKey(clientId) || !playerReadyDictionary[clientId])
            {
                Debug.Log("SetPlayerReadyServerRPC function called");
                allClientsReady = false;
                break;
            }
        }

        //If all clients are ready start countdown;
        if (allClientsReady)
        {
            state.Value = State.CountdownToStart;
        }
        Debug.Log("allClientsReady: " + allClientsReady);

    }

    private void PlayerRespawn_OnPlayerDied()
    {
        Debug.Log("OnPlayersDied function called");
        if (state.Value == State.GamePlaying)
        {
            OnPlayerDiedServerRpc();
            Debug.Log("state changed to waiting: " + state);

        }
    }



    /// <summary>
    /// Tell the server if players are ready 
    /// </summary>
    /// <param name="serverRpcParams"></param>
    [ServerRpc(RequireOwnership = false)]
    private void OnPlayerDiedServerRpc()
    {

        state.Value = State.GameOver;

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
                Debug.Log("GamePlaying");
                break;

            case State.GameOver:
                NetworkManager.Singleton.Shutdown();
                Debug.Log("GameOver");
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

    //Return if the game is over
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

    //called when the tutorial ready button is pressed
    public void OnStartButtonPressed()
    {
        Debug.Log("OnStartButtonPressed function called");
        OnPlayersReady();

    }

    // //called when the player die
    public void OnPlayerDied()
    {
        PlayerRespawn_OnPlayerDied();
    }
}
