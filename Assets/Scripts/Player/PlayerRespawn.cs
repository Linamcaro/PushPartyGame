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
    private float deathPointY = -15f;
  
    private Vector3 respawnPosition;



    private void Start()
    {
        if (!IsOwner) return;
        _instance = this;

    }



    private void Update()
    {
        if (!IsOwner) return;
        RespawnPlayer();
    }

    private void RespawnPlayer()
    {
        
        Debug.Log("PlayerLivesServerRpc called");

        if (transform.position.y < deathPointY)
        {
            PlayerSpawn.Instance.PlayerFell();

            if (PlayerSpawn.Instance.CanSpawn())
            {
                Vector3 respawnTarget = LevelController.Instance.PlatformPosition();
                respawnPosition = new Vector3(0, 1f, respawnTarget.z + 3f);
                //Move player to the respawn position
                transform.position = respawnPosition;
            }
        }

    }
}