using UnityEngine;
using Unity.Netcode;
using System;

public class PlayerRespawn : NetworkBehaviour
{
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

    /// <summary>
    /// Load the winner or gameover scene
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
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
        if (transform.position.y < deathPointY)
        {
            lives--;
            Debug.Log("PlayerLivesServerRpc called");

            PlayerLivesServerRpc();
        }
    }


    /// <summary>
    /// Tell the server the player died
    /// </summary>
    /// <param name="serverRpcParams"></param>
    [ServerRpc(RequireOwnership = false)]
    private void PlayerLivesServerRpc(ServerRpcParams serverRpcParams = default)
    {
       
            lives--;

            if (lives > 1)
            {
                Vector3 respawnTarget = LevelController.Instance.PlatformPosition();

                respawnPosition = new Vector3(0, 1f, respawnTarget.z  + 3f);
                //Move player to the respawn position
                transform.position = respawnPosition;
            }
            else
            {
                lives = 0;
                
            }
    }

   //Returns the player lives
    public int GetPlayerLives()
    {
      return lives;
    }

}
