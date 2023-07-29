using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Services.Lobbies.Models;
using UnityEngine;

public class LobbyUI : MonoBehaviour
{

    [SerializeField] private TMP_InputField joinCodeInputField;
    [SerializeField] private TMP_InputField playerNameInputField;

    [SerializeField] private Transform lobbyContainer;
    [SerializeField] private Transform lobbyTemplate;

    public void MainMenuButton()
    {
        LoadScenes.ChangeScene(LoadScenes.Scene.MainMenu);
    }



    public void JoinGameButton()
    {
        GameLobby.Instance.QuickJoin();
    }

    public void JoinCodeButton()
    {
        GameLobby.Instance.JoinWithCode(joinCodeInputField.text);
    }

    private void Start()
    {
        playerNameInputField.text = MultiplayerManager.Instance.GetPlayerName();

        playerNameInputField.onValueChanged.AddListener((string newText) =>
        {
            MultiplayerManager.Instance.SetPlayerName(newText);
        });

        GameLobby.Instance.OnLobbyListChanged += GameLobby_OnLobbyListChanged;
        UpdateLobbyList(new List<Lobby>());
    }


    private void GameLobby_OnLobbyListChanged(object sender, GameLobby.OnLobbyListChangedEventArgs e)
    {
        UpdateLobbyList(e.lobbyList);
    }

    private void UpdateLobbyList(List<Lobby> lobbyList)
    {
        //clean up
        foreach (Transform child in lobbyContainer)
        {
            if (child == lobbyTemplate) continue;
            Destroy(child.gameObject);
        }

        foreach (Lobby lobby in lobbyList)
        {
            Transform lobbyTransform = Instantiate(lobbyTemplate, lobbyContainer);
            lobbyTransform.gameObject.SetActive(true);
            lobbyTransform.GetComponent<LobbyListSingleUI>().SetLobby(lobby);
        }
    }

    private void OnDestroy()
    {
        GameLobby.Instance.OnLobbyListChanged -= GameLobby_OnLobbyListChanged;
    }

}