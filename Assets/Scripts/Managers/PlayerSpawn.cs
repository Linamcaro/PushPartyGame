using UnityEngine;
using System;
using System.Collections;
public class PlayerSpawn : MonoBehaviour
{

    private static PlayerSpawn _instance;
    public static PlayerSpawn Instance
    {
        get
        {
            return _instance;
        }
    }

    //Event for the game state changes
    public event EventHandler OnLivesChanged;
    public event EventHandler OnPickUpPowerUp;

    public event EventHandler OnPlayerDied;

    //Event for theplayer winner
    public event EventHandler OnPlayerWins;

    //Event for theplayer winner
    public event EventHandler OnPlayerLose;

    public int lives;

    private bool hasLives = true;

    private float inmunityDuration = 20f;
    private float inmunityDuration2 = 1f;

    private void Awake()
    {
        _instance = this;
    }

    private void Start()
    {   
        lives = 2;

      
    }

    private void PlayerRespawn_OnPlayerFell()
    {
        lives--;
        OnLivesChanged?.Invoke(this, EventArgs.Empty);

        if (lives > 0)
        {
            hasLives = true;
        }
        else
        {
            hasLives = false;
            PushPartyGameManager.Instance.OnPlayerDied();

        }

    }

    public void PlayerFell()
    {
        //Debug.Log("Player fell function called");
        PlayerRespawn_OnPlayerFell();
    }

    //Returns if the player has lives to Spawn
    public bool CanSpawn()
    {
        return hasLives;
    }

   
    //Returns the player lives
    public int GetPlayerLives()
    {
        return lives;
    }

    //Increase the player lives
    public int IncreasePlayerLives()
    {

        lives++;
        OnLivesChanged?.Invoke(this, EventArgs.Empty);

        return lives;
    }


    public IEnumerator Inmunity(Collider player)
    {
        player.gameObject.layer = LayerMask.NameToLayer("PlayerInvulnerable");
        yield return new WaitForSeconds(inmunityDuration);

        //player.gameObject.layer = LayerMask.NameToLayer("Player");
    }

    public IEnumerator Inmunity2(Collider player)
    {
        yield return new WaitForSeconds(inmunityDuration2);

        player.gameObject.layer = LayerMask.NameToLayer("Player");
    }

    public void CallInmunity(Collider player)
    {
       
        StartCoroutine(ChangeLayerAndDuration(player));
        OnPickUpPowerUp?.Invoke(this, EventArgs.Empty);
    }

    private IEnumerator ChangeLayerAndDuration(Collider player)
    {
        yield return StartCoroutine(Inmunity(player));

        yield return StartCoroutine(Inmunity2(player));
    }


}
