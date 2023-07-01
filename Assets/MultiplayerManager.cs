using UnityEngine;
using Unity.Netcode;
using System;


public class MultiplayerManager : NetworkBehaviour
{

    //public event EventHandler OnTryingToJoinGame;

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
        connectionApprovalResponse.Approved = true;
    }

    //call this method to start as a client
    public void StartClient()
    {
        //OnTryingToJoinGame?.Invoke(this, EventArgs.Empty);
        Debug.Log("Start as Client");
        NetworkManager.Singleton.StartClient();
        
    }


}
