using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using Unity.Services.Authentication;
using Unity.Services.Core;
using Unity.Services.Lobbies;
using Unity.Services.Lobbies.Models;
using UnityEngine;

public class GameLobby : MonoBehaviour
{
    public static GameLobby Instance { get; private set; }

    private Lobby joinedLobby;
    private float heatBeatTimer;

    private void Awake()
    {
        Instance = this;
        DontDestroyOnLoad(gameObject);

        InitializeUnityAuthentication();
    }

    ///Authentication
    private async void InitializeUnityAuthentication()
    {
        if (UnityServices.State != ServicesInitializationState.Initialized)
        {
            InitializationOptions initializationOptions = new InitializationOptions();
            //Initialize with different name everytime
            initializationOptions.SetProfile(Random.Range(0, 10000).ToString());
            await UnityServices.InitializeAsync(initializationOptions);

            await AuthenticationService.Instance.SignInAnonymouslyAsync();
        }
    }


    ///Create and join a Lobby
    public async void CreateLobby(string lobbyName, bool isPrivate)
    {
        //returns a lobby
        try
        {
            joinedLobby = await LobbyService.Instance.CreateLobbyAsync(lobbyName, MultiplayerManager.MAX_PLAYERS, new CreateLobbyOptions
            {
                IsPrivate = isPrivate,

            });

            MultiplayerManager.Instance.StartHost();
            LoadScenes.LoadTagetScene(LoadScenes.Scene.CharacterSelection);

        }
        catch (LobbyServiceException e)
        {
            Debug.Log(e);
        }

    }

    ///Join the lobby
    public async void QuickJoin()
    {
        try
        {
            joinedLobby = await LobbyService.Instance.QuickJoinLobbyAsync();

            MultiplayerManager.Instance.StartClient();

        }
        catch (LobbyServiceException e)
        {
            Debug.Log(e);
        }
    }

    private void Update()
    {
        HandleHeartbeat();
    }

    private void HandleHeartbeat()
    {
        if (IsLobbyHost())
        {
            heatBeatTimer -= Time.deltaTime;
            if(heatBeatTimer < 0)
            {
                float heatBeatTimerMax = 15f;
                heatBeatTimer = heatBeatTimerMax;


                LobbyService.Instance.SendHeartbeatPingAsync(joinedLobby.Id);
            }

        }
    }

    //helper function to check if it is the host
    private bool IsLobbyHost()
    {
        return joinedLobby != null && joinedLobby.HostId == AuthenticationService.Instance.PlayerId;
    }


    /// <summary>
    /// Return the code joined lobby 
    /// </summary>
    /// <param name="lobbyCode"></param>
    public async void JoinWithCode(string lobbyCode)
    {
        try 
        {
            joinedLobby = await LobbyService.Instance.JoinLobbyByCodeAsync(lobbyCode);

            MultiplayerManager.Instance.StartClient();
        }
        catch (LobbyServiceException e)
        {
            Debug.Log(e);
        }
    }

    /// <summary>
    /// Clean up and delete lobby when the game starts;
    /// </summary>
    public async void DeleteLobby()
    {
        if (joinedLobby != null)
        {
            try
            {
                await LobbyService.Instance.DeleteLobbyAsync(joinedLobby.Id);

                joinedLobby = null;
            }
            catch (LobbyServiceException e)
            {
                Debug.Log(e);
            }
        }
    }

    /// <summary>
    /// Remove player
    /// </summary>
    public async void LeaveLobby()
    {
        if (joinedLobby != null)
        {
            try
            {
              await LobbyService.Instance.RemovePlayerAsync(joinedLobby.Id, AuthenticationService.Instance.PlayerId);

                joinedLobby = null;
                
            } catch(LobbyServiceException e)
            {
                Debug.Log(e);
            }
        }

    }

    /// <summary>
    /// Remove player
    /// </summary>
    public async void KickPlayer(string playerId)
    {
        if (IsLobbyHost())
        {
            try
            {
                await LobbyService.Instance.RemovePlayerAsync(joinedLobby.Id, playerId);

            }
            catch (LobbyServiceException e)
            {
                Debug.Log(e);
            }
        }

    }

    public Lobby GetLobby()
    {
        return joinedLobby;
    }
}