using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class CharacterSelectReady : NetworkBehaviour
{
    private static CharacterSelectReady _instance;
    public static CharacterSelectReady Instance
    {
        get
        {
            return _instance;
        }
    }

    public event EventHandler OnReadyChanged;

    //Store the player ID and if it is ready
    private Dictionary<ulong, bool> playerReadyDictionary;

    private void Awake()
    {
        _instance = this;
        playerReadyDictionary = new Dictionary<ulong, bool>();
    }


    public void PlayerReady()
    {
        SetPlayerReadyServerRpc();
    }

    /// <summary>
    /// Tell the server if players are ready 
    /// </summary>
    /// <param name="serverRpcParams"></param>
    [ServerRpc(RequireOwnership = false)]
    private void SetPlayerReadyServerRpc(ServerRpcParams serverRpcParams = default)
    {
        SetPlayerReadyClientRpc(serverRpcParams.Receive.SenderClientId);

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

        if (allClientsReady)
        {
            LoadScenes.LoadTagetScene(LoadScenes.Scene.MainScene);
        }
        Debug.Log("allClientsReady: " + allClientsReady);

    }

    [ClientRpc]
    private void SetPlayerReadyClientRpc(ulong clientId)
    {
        playerReadyDictionary[clientId] = true;

        OnReadyChanged?.Invoke(this, EventArgs.Empty);
    }

    //Check if client is ready
    public bool IsPlayerReady(ulong clientId)
    {
        return playerReadyDictionary.ContainsKey(clientId) && playerReadyDictionary[clientId];
    }


}
