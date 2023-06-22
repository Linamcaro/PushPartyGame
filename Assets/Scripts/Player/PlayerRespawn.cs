using UnityEngine;
using Unity.Netcode;

public class PlayerRespawn : NetworkBehaviour
{

    //lives
    private float deathPointY;
    private int lives;

    private Vector3 respawnPosition;


    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();
       
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

}
