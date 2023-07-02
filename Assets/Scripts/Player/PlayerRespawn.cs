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

    //Event for the game state changes
    public event EventHandler OnLivesChanged;

    //lives
    private float deathPointY = -15f;
    public int lives = 2;

    private Vector3 respawnPosition;

    private float inmunityDuration = 20f;
    private float inmunityDuration2 = 1f;


    private void Awake()
    {
        _instance = this;

    }

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
            if (lives > 0)
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
        Debug.Log("PlayerLivesServerRpc called");

        if (transform.position.y < deathPointY)
        {
            Vector3 respawnTarget = LevelController.Instance.PlatformPosition();

            lives--;
            OnLivesChanged?.Invoke(this, EventArgs.Empty);

            if (lives > 0)
            {
                respawnPosition = new Vector3(0, 1f, respawnTarget.z + 3f);
                //Move player to the respawn position
                transform.position = respawnPosition;
            }
            else
            {
                lives = 0;
                PlayerLivesServerRpc();

            }
        }

    }


    /// <summary>
    /// Tell the server the player died
    /// </summary>
    /// <param name="serverRpcParams"></param>
    [ServerRpc(RequireOwnership = false)]
    private void PlayerLivesServerRpc(ServerRpcParams serverRpcParams = default)
    {

        LoadScenes.LoadTagetScene(LoadScenes.Scene.GameOver);

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