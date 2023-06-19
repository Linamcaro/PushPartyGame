using UnityEngine;


public class PlayerRespawn : MonoBehaviour
{
    [SerializeField] private float deathPointY;
    [SerializeField] private int lives;

    private Vector3 respawnPosition;

    private void Start()
    {
        
        lives = 2;

        GenerateRespawnPosition();
        deathPointY = -7f;
    }

    private void GenerateRespawnPosition()
    {
        // Lógica para generar el punto de respawn (ejemplo: el centro de la pista)
        respawnPosition = new Vector3(CalculateCenterOfTrack(), 0f, transform.position.z);

        
    }

    private float CalculateCenterOfTrack()
    {
        // Logic to calculare the center of the track
        return 0f;
    }

    private void RespawnPlayer()
    {
        if (transform.position.y < deathPointY)
        {
            lives--;

            if (lives > 0)
            {
                //Move player to the respawn position
                transform.position = respawnPosition;
            }
        }
        else
        {
           
        }
    }

    
}
