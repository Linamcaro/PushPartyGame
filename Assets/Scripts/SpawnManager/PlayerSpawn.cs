/*using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerSpawn : NetworkBehaviour
{
    [SerializeField] private GameObject player;

    private void Start()
    {
        DontDestroyOnLoad(this.gameObject);
    }
    public override void OnNetworkSpawn()
    {
        //When the scenes loads call SceneLoaded
        NetworkManager.Singleton.SceneManager.OnLoadEventCompleted += SceneLoaded;

    }

    /// <summary>
    /// Spawn players if there is a host
    /// </summary>
    /// <param name="sceneName"></param>
    /// <param name="loadSceneMode"></param>
    /// <param name="clientsCompleted"></param>
    /// <param name="clientsTimedOut"></param>
    private void SceneLoaded(string sceneName, LoadSceneMode loadSceneMode, List<ulong> clientsCompleted, List<ulong> clientsTimedOut)
    {
        if(IsHost && sceneName == "MainScene")
        {
            int position = -3;
            foreach (ulong id in clientsCompleted)
            {

                GameObject playerSpawn = Instantiate(player);
                playerSpawn.transform.position = new Vector3(position, 0, 0);
                playerSpawn.GetComponent<NetworkObject>().SpawnAsPlayerObject(id, true);
                position += 6;
            }
        }
    }
}*/
