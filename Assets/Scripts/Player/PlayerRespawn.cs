using UnityEngine;
using Unity.Netcode;
using System;
using System.Collections;
using System.Collections.Generic;

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

    //Call this event when lives changed
    public event EventHandler OnLivesChanged;

    public event EventHandler OnNoLives;
    //lives
    private float deathPointY = -15f;
    public int lives = 2;

    private Vector3 respawnPosition;

    private float inmunityDuration = 20f;
    private float inmunityDuration2 = 1f;
    private Dictionary<ulong, bool> playerDiedDictionary;


    private void Awake()
    {
        if (!IsOwner) return;
        _instance = this;

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
            if(lives > 0 && IsOwner)
            {
                LoadScenes.LoadTagetScene(LoadScenes.Scene.Winner);
            }
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
        playerDiedDictionary[serverRpcParams.Receive.SenderClientId] = true;

        lives--;
        OnLivesChanged?.Invoke(this, EventArgs.Empty);

        if (lives > 0)
        {
            Vector3 respawnTarget = LevelController.Instance.PlatformPosition();
            respawnPosition = new Vector3(0, 1f, respawnTarget.z + 3f);
            //Move player to the respawn position
            transform.position = respawnPosition;
        }
        else
        {
           lives = 0;
           OnNoLives?.Invoke(this, EventArgs.Empty);

        }

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

        player.gameObject.layer = LayerMask.NameToLayer("Player");
    }

    public IEnumerator Inmunity2(Collider player)
    {
        yield return new WaitForSeconds(inmunityDuration2);

        player.gameObject.layer = LayerMask.NameToLayer("Player");
    }

    public void CallInmunity(Collider player)
    {
        StartCoroutine(ChangeLayerAndDuration(player));
    }

    private IEnumerator ChangeLayerAndDuration(Collider player)
    {
        yield return StartCoroutine(Inmunity(player));
        yield return StartCoroutine(Inmunity2(player));
    }

}
