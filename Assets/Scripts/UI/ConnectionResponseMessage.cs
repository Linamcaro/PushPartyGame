using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class ConnectionResponseMessage : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI messageText;
    [SerializeField] private GameObject closeButton;


    private void Start()
    {
        MultiplayerManager.Instance.OnFailedToJoinGame += MultiplayerManagerr_OnFailedToJoinGame;
        GameLobby.Instance.OnCreateLobbyStarted += MultiplayerManager_OnCreateLobbyStarted;
        GameLobby.Instance.OnCreateLobbyFailed += MultiplayerManager_OnCreateLobbyFailed;
        GameLobby.Instance.OnJoinStarted += MultiplayerManager_OnJoinStarted;
        GameLobby.Instance.OnJoinFailed += MultiplayerManager_OnJoinFailed;
        GameLobby.Instance.OnQuickJoinFailed += MultiplayerManager_OnQuickJoinFailed;

        Hide();
        hideButton();


    }

    private void MultiplayerManager_OnQuickJoinFailed(object sender, System.EventArgs e)
    {
        ShowMessage("Could not find a Game to Quick Join!");
        ShowButton();
    }

    private void MultiplayerManager_OnJoinFailed(object sender, System.EventArgs e)
    {
        ShowMessage("Failed to join Game!");
        ShowButton();
    }

    private void MultiplayerManager_OnJoinStarted(object sender, System.EventArgs e)
    {
        ShowMessage("Joining Game...");
        hideButton();
    }

    private void MultiplayerManager_OnCreateLobbyFailed(object sender, System.EventArgs e)
    {
        ShowMessage("Failed to create Game!");
        ShowButton();
    }

    private void MultiplayerManager_OnCreateLobbyStarted(object sender, System.EventArgs e)
    {
        ShowMessage("Creating Game...");
        hideButton();
    }

    private void MultiplayerManagerr_OnFailedToJoinGame(object sender, System.EventArgs e)
    {
        if (NetworkManager.Singleton.DisconnectReason == "")
        {
            ShowMessage("Connection Failed!");
            ShowButton();
        }
        else
        {
            ShowMessage(NetworkManager.Singleton.DisconnectReason);
            ShowButton();
        }
    }

    private void ShowMessage(string message)
    { 
        messageText.text = message;
        Show();
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

    private void ShowButton()
    {
        closeButton.SetActive(true);
    }

    private void hideButton()
    {
        closeButton.SetActive(false);
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
