using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Netcode;
using Unity.Services.Lobbies.Models;
using UnityEngine;

public class CharacterSelectUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI lobbyNameText;
    [SerializeField] private TextMeshProUGUI lobbyCodeText;
    public void MainMenuButton()
    { 
        NetworkManager.Singleton.Shutdown();
        LoadScenes.ChangeScene(LoadScenes.Scene.MainMenu);
    }

    public void ReadyButton()
    {
        CharacterSelectReady.Instance.PlayerReady();
    }

    private void Start()
    {
        Lobby lobby = GameLobby.Instance.GetLobby();
        lobbyNameText.text = "Lobby Name: " + lobby.Name;
        lobbyCodeText.text = "Lobby Code: " + lobby.LobbyCode;

    }
}
