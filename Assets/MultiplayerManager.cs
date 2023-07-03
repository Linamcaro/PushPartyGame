using UnityEngine;
using Unity.Netcode;
using System;
using UnityEngine.SceneManagement;

public class MultiplayerManager : NetworkBehaviour
{

    public event EventHandler OnTryingToJoinGame;
    public event EventHandler OnFailedToJoinGame;


    private const int MAX_PLAYERS = 2;

    private static MultiplayerManager _instance;
    public static MultiplayerManager Instance
    {
        get
        {
            return _instance;
        }
    }

    private void Awake()
    {
        _instance = this;

        DontDestroyOnLoad(gameObject);
    }


    //call this method to start as a host
    public void StartHost()
    {
        NetworkManager.Singleton.ConnectionApprovalCallback += NetworkManager_ConnectionApprovalCallback;
        Debug.Log("Start as Host");
        NetworkManager.Singleton.StartHost();
    }

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
}
