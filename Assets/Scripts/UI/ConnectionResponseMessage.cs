using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Netcode;
using UnityEngine;

public class ConnectionResponseMessage : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI messageText;

 

    private void Start()
    {
        GameLobby.Instance.OnCreateLobbyStarted += MultiplayerManager_OnCreateLobbyStarted;
        GameLobby.Instance.OnCreateLobbyFailed += MultiplayerManager_OnCreateLobbyFailed;
        GameLobby.Instance.OnJoinStarted += MultiplayerManager_OnJoinStarted;
        GameLobby.Instance.OnJoinFailed += MultiplayerManager_OnJoinFailed;
        GameLobby.Instance.OnQuickJoinFailed += MultiplayerManager_OnQuickJoinFailed;

        Hide();
    }

    private void MultiplayerManager_OnQuickJoinFailed(object sender, System.EventArgs e)
    {
        ShowMessage("Could not find a Game to Quick Join!");
    }

    private void MultiplayerManager_OnJoinFailed(object sender, System.EventArgs e)
    {
        ShowMessage("Failed to join Game!");
    }

    private void MultiplayerManager_OnJoinStarted(object sender, System.EventArgs e)
    {
        ShowMessage("Joining Game...");
    }

    private void MultiplayerManager_OnCreateLobbyFailed(object sender, System.EventArgs e)
    {
        ShowMessage("Failed to create Game!");
    }

    private void MultiplayerManager_OnCreateLobbyStarted(object sender, System.EventArgs e)
    {
        ShowMessage("Creating Game...");
    }

    private void MultiplayerManagerr_OnFailedToJoinGame(object sender, System.EventArgs e)
    {
        if (NetworkManager.Singleton.DisconnectReason == "")
        {
            ShowMessage("Failed to connect");
        }
        else
        {
            ShowMessage(NetworkManager.Singleton.DisconnectReason);
        }
    }

    private void ShowMessage(string message)
    {
        Show();
        messageText.text = message;
    }

    private void Show()
    {
        gameObject.SetActive(true);
    }

    //Hide UI
    private void Hide()
    {
        gameObject.SetActive(false);
    }

    private void OnDestroy()
    {
        GameLobby.Instance.OnCreateLobbyStarted -= MultiplayerManager_OnCreateLobbyStarted;
        GameLobby.Instance.OnCreateLobbyFailed -= MultiplayerManager_OnCreateLobbyFailed;
        GameLobby.Instance.OnJoinStarted -= MultiplayerManager_OnJoinStarted;
        GameLobby.Instance.OnJoinFailed -= MultiplayerManager_OnJoinFailed;
        GameLobby.Instance.OnQuickJoinFailed -= MultiplayerManager_OnQuickJoinFailed;
    }

}
