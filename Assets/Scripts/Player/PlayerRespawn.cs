using UnityEngine;
using Unity.Netcode;

public class PlayerRespawn : NetworkBehaviour
{
    private static PlayerRespawn _instance;
    public static PlayerRespawn Instance
    {
        get { return _instance; }
    }

    //lives
    private float deathPointY;
    private int lives;

    private Vector3 respawnPosition;


    public override void OnNetworkSpawn()
    {
        if (!IsOwner) return;

        base.OnNetworkSpawn();
           _instance = this;

            lives = 2;
            deathPointY = -15f;
           
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

           if (lives > 0)
           {
             respawnPosition = new Vector3(0, 1f, transform.position.z);
             //Move player to the respawn position
             transform.position = respawnPosition;
           }
           else
           {
              Debug.Log("Game Over");


           }
        }
    }

    //Returns the player lives
    public int GetPlayerLives()
    {
        return lives;
    }


}
