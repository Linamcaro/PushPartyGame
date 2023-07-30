using UnityEngine;
using Unity.Netcode;
using System;
using UnityEngine.SceneManagement;
using Unity.Services.Authentication;

public class MultiplayerManager : NetworkBehaviour
{

    public const int MAX_PLAYERS = 2;
    public const string PLAYER_PREFS_PLAYER_NAME_MULTIPLAYER = "PlayerNameMultiplayer";

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
    private string playerName;


    private void Awake()
    {
        _instance = this;
        DontDestroyOnLoad(gameObject);

        playerName = PlayerPrefs.GetString(PLAYER_PREFS_PLAYER_NAME_MULTIPLAYER, "PlayerName" + UnityEngine.Random.Range(100,1000));

        playerDataNetworkList = new NetworkList<PlayerData>();

        playerDataNetworkList.OnListChanged += PlayerDataNetworkList_OnListChanged;

    }



    public void SetPlayerName(string playerName)
    {
        this.playerName = playerName;
        PlayerPrefs.SetString(PLAYER_PREFS_PLAYER_NAME_MULTIPLAYER, playerName);

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
        NetworkManager.Singleton.OnClientDisconnectCallback += NetworkManager_Server_OnClientDisconnectCallback;

        NetworkManager.Singleton.StartHost();
        Debug.Log("Start as Host");
    }

    private void NetworkManager_Server_OnClientDisconnectCallback(ulong clientId)
    {
        for (int i = 0; i < playerDataNetworkList.Count; i++)
        {
            PlayerData playerData = playerDataNetworkList[i];
            if (playerData.clientId == clientId)
            {
                // Disconnected!
                playerDataNetworkList.RemoveAt(i);
            }
        }
    }

    private void NetworkManager_OnClientConnectedCallback(ulong clientId)
    {
        Debug.Log("On client connected callback");

        playerDataNetworkList.Add(new PlayerData
        {
            clientId = clientId
            
         });

        setPlayerNameServerRpc(GetPlayerName());
        setPlayerIdServerRpc(AuthenticationService.Instance.PlayerId);

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

        NetworkManager.Singleton.OnClientDisconnectCallback += NetworkManager_client_OnClientDisconnectCallback;
        NetworkManager.Singleton.OnClientConnectedCallback += NetworkManager_client_OnClientConnectedCallback;
        NetworkManager.Singleton.StartClient();
        
    }

    private void NetworkManager_client_OnClientConnectedCallback(ulong clientId)
    {
        setPlayerNameServerRpc(GetPlayerName());

        setPlayerIdServerRpc(AuthenticationService.Instance.PlayerId);

    }

    //send player name to the server
    [ServerRpc(RequireOwnership = false)]
    private void setPlayerNameServerRpc(String playerName, ServerRpcParams serverRpcParams = default)
    {
        int playerDataIndex = GetPlayerDataIndexFromClientID(serverRpcParams.Receive.SenderClientId);

        PlayerData playerData = playerDataNetworkList[playerDataIndex];

        playerData.playerName = playerName;

        playerDataNetworkList[playerDataIndex] = playerData;
    }

    //send player id to the server
    [ServerRpc(RequireOwnership = false)]
    private void setPlayerIdServerRpc(String playerId, ServerRpcParams serverRpcParams = default)
    {
        int playerDataIndex = GetPlayerDataIndexFromClientID(serverRpcParams.Receive.SenderClientId);

        PlayerData playerData = playerDataNetworkList[playerDataIndex];

        playerData.playerId = playerId;

        playerDataNetworkList[playerDataIndex] = playerData;
    }


    private void NetworkManager_client_OnClientDisconnectCallback(ulong clientId)
    {
        OnFailedToJoinGame?.Invoke(this, EventArgs.Empty);
    }

    public int GetPlayerDataIndexFromClientID(ulong clientId)
    {
        for (int i = 0; i < playerDataNetworkList.Count; i++)
        {
            if (playerDataNetworkList[i].clientId == clientId)
            {
                return i;
            }
        }
        return -1;
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

    public string GetPlayerName()
    {
        return playerName;
    }
    public void KickPlayer(ulong clientId)
    {
        NetworkManager.Singleton.DisconnectClient(clientId);
        NetworkManager_Server_OnClientDisconnectCallback(clientId);
    }
}


