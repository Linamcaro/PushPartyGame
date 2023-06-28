using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class PlayersReadyTest : NetworkBehaviour
{
    //Store the player ID and if it is ready
    private Dictionary<ulong, bool> playerReadyDictionary;

    private void Awake()
    {
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
}