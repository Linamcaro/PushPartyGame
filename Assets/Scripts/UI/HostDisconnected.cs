using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class HostDisconnected : MonoBehaviour
{
    [SerializeField] private GameObject hostDisconnectedUI;
    [SerializeField] private Button playAgain;

    void Awake()
    {
        playAgain.onClick.AddListener(() =>
        {
            NetworkManager.Singleton.Shutdown();
            LoadScenes.ChangeScene(LoadScenes.Scene.Lobby);
        });

        Hide();
    }

    private void Start()
    {
        NetworkManager.Singleton.OnClientDisconnectCallback += NetworkManager_OnClientConnectedCallback;
        
    }

    /// <summary>
    /// Check if the serve shutdown
    /// </summary>
    /// <param name="clientId"></param>
    private void NetworkManager_OnClientConnectedCallback(ulong clientId)
    {
        if (clientId == NetworkManager.ServerClientId)
        {
            Show();
        }
    }
                 

    private void Show()
    {
        hostDisconnectedUI.SetActive(true);
    }

    private void Hide()
    {
        hostDisconnectedUI.SetActive(false);
    }


}
