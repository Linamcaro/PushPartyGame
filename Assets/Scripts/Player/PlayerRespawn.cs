using UnityEngine;
using Unity.Netcode;
using System;

public class PlayerRespawn : NetworkBehaviour
{
  
    //lives
    private float deathPointY = -15f;
    private int lives = 2;

    private Vector3 respawnPosition;


    public override void OnNetworkSpawn()
    {
        if (!IsOwner) return;

        base.OnNetworkSpawn();
           
    }

    private void Start()
    {
        PushPartyGameManager.Instance.OnStateChanged += PushPartyGameManager_OnStateChanged;

    }

    private void PushPartyGameManager_OnStateChanged(object sender, EventArgs e)
    {
        Debug.Log("PushPartyGameManager_OnStateChanged called");

        if (PushPartyGameManager.Instance.IsGameOver())
        {
            if(lives > 0)
            {
                LoadScenes.LoadTagetScene(LoadScenes.Scene.Winner);
            }
            else
            {
                LoadScenes.LoadTagetScene(LoadScenes.Scene.GameOver);
            }
        }
    }

    private void Update()
    {
        
       RespawnPlayer();
    }

    private void RespawnPlayer()
    {
        if (!IsOwner) return;
        PlayerLivesServerRpc();

    }


    /// <summary>
    /// Tell the server the player died
    /// </summary>
    /// <param name="serverRpcParams"></param>
    [ServerRpc(RequireOwnership = false)]
    private void PlayerLivesServerRpc(ServerRpcParams serverRpcParams = default)
    {
        Debug.Log("PlayerLivesServerRpc called");

        if (transform.position.y < deathPointY)
        {
            Vector3 respawnTarget = LevelController.Instance.PlatformPosition();

            lives--;

            if (lives > 0)
            {
                respawnPosition = new Vector3(0, 1f, respawnTarget.z);
                //Move player to the respawn position
                transform.position = respawnPosition;
            }
            else
            {
                lives = 0;
                LoadScenes.LoadTagetScene(LoadScenes.Scene.GameOver);

            }
        }
    }

   //Returns the player lives
    public int GetPlayerLives()
    {
      return lives;
    }

}
