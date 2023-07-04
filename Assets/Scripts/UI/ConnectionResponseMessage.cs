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
        MultiplayerManager.Instance.OnFailedToJoinGame += MultiplayerManager_OnFailedToJoinGame;

        Hide();
    }

    private void MultiplayerManager_OnFailedToJoinGame(object sender, EventArgs e)
    {
        Show();
        messageText.text = NetworkManager.Singleton.DisconnectReason;

        if (messageText.text == "")
        {
            messageText.text = "Connection failed";
        }

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
        MultiplayerManager.Instance.OnFailedToJoinGame -= MultiplayerManager_OnFailedToJoinGame;
    }

}
