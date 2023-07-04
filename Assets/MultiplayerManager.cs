using UnityEngine;
using Unity.Netcode;
using System;
using UnityEngine.SceneManagement;

public class MultiplayerManager : NetworkBehaviour
{

    private const int MAX_PLAYERS = 2;

    private static MultiplayerManager _instance;
    public static MultiplayerManager Instance
    {
        get
        {
            return _instance;
        }
    }


    public event EventHandler OnTryingToJoinGame;
    public event EventHandler OnFailedToJoinGame;
    public event EventHandler OnPlayerDataNetworkListChanged;

    private NetworkList<PlayerData> playerDataNetworkList;


    private void Awake()
    {
        _instance = this;
        DontDestroyOnLoad(gameObject);

        playerDataNetworkList = new NetworkList<PlayerData>();

        playerDataNetworkList.OnListChanged += PlayerDataNetworkList_OnListChanged;

    }

    private void PlayerDataNetworkList_OnListChanged(NetworkListEvent<PlayerData> changeEvent)
    {
        OnPlayerDataNetworkListChanged?.Invoke(this, EventArgs.Empty);

        Debug.Log("PlayerDataNetworkList_OnListChanged function called from the MultiplayerManager script");
    }


    //call this method to start as a host
    public void StartHost()
    {
        NetworkManager.Singleton.ConnectionApprovalCallback += NetworkManager_ConnectionApprovalCallback;
        NetworkManager.Singleton.OnClientConnectedCallback += NetworkManager_OnClientConnectedCallback;

        NetworkManager.Singleton.StartHost();
        Debug.Log("Start as Host");
    }

    private void NetworkManager_OnClientConnectedCallback(ulong clientId)
    {
        Debug.Log("On client connected callback");

        playerDataNetworkList.Add(new PlayerData
        {
            clientId = clientId
            
         });

    }

    /// <summary>
    /// Method to handle client connection approvals
    /// </summary>
    /// <param name="ConnectionApprovalRequest"></param>
    /// <param name="connectionApprovalResponse"></param>
    private void NetworkManager_ConnectionApprovalCallback(NetworkManager.ConnectionApprovalRequest ConnectionApprovalRequest, NetworkManager.ConnectionApprovalResponse connectionApprovalResponse)
    {
        if (SceneManager.GetActiveScene().name != LoadScenes.Scene.CharacterSelection.ToString())
        {
            connectionApprovalResponse.Approved = false;
            connectionApprovalResponse.Reason = "Game has already started";
            return;
        }

        if (NetworkManager.Singleton.ConnectedClientsIds.Count >= MAX_PLAYERS)
        {
            connectionApprovalResponse.Approved = false;
            connectionApprovalResponse.Reason = "Can not accept more players";
            return;
        }

        connectionApprovalResponse.Approved = true;

    }

    ///call this method to start as a client
    public void StartClient()
    {
        OnTryingToJoinGame?.Invoke(this, EventArgs.Empty);

        Debug.Log("Start as Client");

        NetworkManager.Singleton.OnClientDisconnectCallback += NetworkManager_OnClientDisconnectCallback;
        NetworkManager.Singleton.StartClient();
        
    }

    private void NetworkManager_OnClientDisconnectCallback(ulong clientId)
    {
        OnFailedToJoinGame?.Invoke(this, EventArgs.Empty);
    }


    public bool IsPlayerIndexConnected(int playerIndex)
    {
        Debug.Log("Player index connected function called from the Multiplayer Manager script");
        return playerIndex < playerDataNetworkList.Count;
    }

    public PlayerData GetPlayerDataFromPlayerIndex(int playerIndex)
    {
        return playerDataNetworkList[playerIndex];
    }



}


