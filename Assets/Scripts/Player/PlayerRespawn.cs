using UnityEngine;
using Unity.Netcode;
using System;
using System.Collections;
public class PlayerRespawn : NetworkBehaviour
{

    private static PlayerRespawn _instance;
    public static PlayerRespawn Instance
    {
        get
        {
            return _instance;
        }
    }

    //lives
    private float deathPointY = -10f;
  
    private Vector3 respawnPosition;
    public bool isFalling { get; private set; }

    public event EventHandler OnPlayerFell;

    private float respawnTarget;

    private void Start()
    {
        if (!IsOwner) return;
        _instance = this;
        isFalling = false;
    }

    private void Update()
    {
        if (!IsOwner) return;
        RespawnPlayer();
    }

    private void RespawnPlayer()
    {
        
        //Debug.Log("PlayerLivesServerRpc called");

        if (transform.position.y < deathPointY)
        {
            OnPlayerFell?.Invoke(this, EventArgs.Empty);
            PlayerSpawn.Instance.PlayerFell();
            isFalling = true;

            if (PlayerSpawn.Instance.CanSpawn())
            {
                respawnTarget = LevelController.Instance.PlatformPosition();
                respawnPosition = new Vector3(0, 2f, respawnTarget + 20f);
                //Move player to the respawn position
                transform.position = respawnPosition;
                isFalling = false;
            }
            else
            {
                respawnPosition = new Vector3(133, 2f , -90);
                transform.position = respawnPosition;
            }
        }

    }

  
}