using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using Unity.Networking.Transport.Relay;
using Unity.Services.Authentication;
using Unity.Services.Core;
using Unity.Services.Lobbies;
using Unity.Services.Lobbies.Models;
using Unity.Services.Relay;
using Unity.Services.Relay.Models;
using UnityEngine;

public class GameLobby : MonoBehaviour
{
    private const string KEY_RELAY_JOIN_CODE = "RelayJoinCode";
    public static GameLobby Instance { get; private set; }

    private Lobby joinedLobby;
    private float heatBeatTimer;
    private float ListLobbiesTimer;

    public event EventHandler OnJoinFailed;
    public event EventHandler OnLobbyListChanged;
    public class OnLobbyListChangedEventArgs : EventArgs
    {
        public List<Lobby> lobbyList;
    }


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

    /// <summary>
    /// allocate Relay
    /// </summary>
    /// <returns></returns>
    private async Task<Allocation> AllocateRelay()
    {
        try
        {
            Allocation allocation = await RelayService.Instance.CreateAllocationAsync(MultiplayerManager.MAX_PLAYERS - 1);

            return allocation;
        }
        catch (RelayServiceException e)
        {
            Debug.Log(e);

            return default;
        }
    }


    private async Task<string> GetRelayJoinCode(Allocation allocation)
    {
        try
        {
            string relayJoinCode = await RelayService.Instance.GetJoinCodeAsync(allocation.AllocationId);

            return relayJoinCode;
        }
        catch (RelayServiceException e)
        {
            Debug.Log(e);
            return default;
        }
    }

    private async Task<JoinAllocation> JoinRelay(string joinCode)
    {
        try
        {
            JoinAllocation joinAllocation = await RelayService.Instance.JoinAllocationAsync(joinCode);
            return joinAllocation;
        }
        catch (RelayServiceException e)
        {
            Debug.Log(e);
            return default;
        }
    }

    ///Create and join a Lobby
    public async void CreateLobby(string lobbyName, bool isPrivate)
    {
        
        try
        {
            //returns a lobby
            joinedLobby = await LobbyService.Instance.CreateLobbyAsync(lobbyName, MultiplayerManager.MAX_PLAYERS, new CreateLobbyOptions
            {
                IsPrivate = isPrivate,

            });

            Allocation allocation = await AllocateRelay();

            string relayJoinCode = await GetRelayJoinCode(allocation);

            await LobbyService.Instance.UpdateLobbyAsync(joinedLobby.Id, new UpdateLobbyOptions
            {
                Data = new Dictionary<string, DataObject> {
                     { KEY_RELAY_JOIN_CODE , new DataObject(DataObject.VisibilityOptions.Member, relayJoinCode) }
                 }
            });

            //Start Connection through this relay
            NetworkManager.Singleton.GetComponent<UnityTransport>().SetRelayServerData(new RelayServerData(allocation, "dtls"));

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

            string relayJoinCode = joinedLobby.Data[KEY_RELAY_JOIN_CODE].Value;

            JoinAllocation joinAllocation = await JoinRelay(relayJoinCode);

            NetworkManager.Singleton.GetComponent<UnityTransport>().SetRelayServerData(new RelayServerData(joinAllocation, "dtls"));

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
        //HandlePeriodicListLobbies();
    }

    /*private void HandlePeriodicListLobbies()
    {
        if (joinedLobby == null && AuthenticationService.Instance.IsSignedIn)
        {
            ListLobbiesTimer -= Time.deltaTime;
            if (ListLobbiesTimer <= 0f)
            {
                float listLobbiesTimerMax = 3;
                ListLobbiesTimer = listLobbiesTimerMax;
                ListLobbies();
            }
        }
    }*/


    private void HandleHeartbeat()
    {
        if (IsLobbyHost())
        {
            heatBeatTimer -= Time.deltaTime;
            if (heatBeatTimer < 0)
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

    /*private async void ListLobbies()
    {
        try
        {
            QueryLobbiesOptions queryLobbiesOptions = new QueryLobbiesOptions
            {
                Filters = new List<QueryFilter>
            {
                new QueryFilter(QueryFilter.FieldOptions.AvailableSlots,"0", QueryFilter.OpOptions.GT)
            }
            };
            QueryResponse queryResponse = await LobbyService.Instance.QueryLobbiesAsync(queryLobbiesOptions);
            OnLobbyListChanged?.Invoke(this, new OnLobbyListChangedEventArgs
            {

                lobbyList = queryResponse.Results

            });
        }
        catch (LobbyServiceException e)
        {
            Debug.Log(e);
        }
    }*/


    /// <summary>
    /// Return the code joined lobby 
    /// </summary>
    /// <param name="lobbyCode"></param>
    public async void JoinWithCode(string lobbyCode)
    {
        try
        {
            joinedLobby = await LobbyService.Instance.JoinLobbyByCodeAsync(lobbyCode);

            string relayJoinCode = joinedLobby.Data[KEY_RELAY_JOIN_CODE].Value;

            JoinAllocation joinAllocation = await JoinRelay(relayJoinCode);

            NetworkManager.Singleton.GetComponent<UnityTransport>().SetRelayServerData(new RelayServerData(joinAllocation, "dtls"));

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