using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class CharacterSelectPlayer : MonoBehaviour
{

    [SerializeField] private int playerIndex;
    [SerializeField] private GameObject readyGameObject;
    [SerializeField] private GameObject playerLight;
    [SerializeField] private TextMeshPro playerNameText;
    [SerializeField] private Button KickButton;

   /* private void Awake()
    {
        KickButton.onClick.AddListener(() =>
        {
            PlayerData playerData = MultiplayerManager.Instance.GetPlayerDataFromPlayerIndex(playerIndex);
            GameLobby.Instance.KickPlayer(playerData.playerId.ToString());
            MultiplayerManager.Instance.KickPlayer(playerData.clientId);
        });
    
    }*/


        private void Start()
    {
        MultiplayerManager.Instance.OnPlayerDataNetworkListChanged += MultiplayerManager_OnPlayerDataNetworkListChanged;
        CharacterSelectReady.Instance.OnReadyChanged += CharacterSelectReady_OnreadyChanged;
        UpdatePlayer();
    }

    private void CharacterSelectReady_OnreadyChanged(object sender, EventArgs e)
    {
        UpdatePlayer();
    }

    private void MultiplayerManager_OnPlayerDataNetworkListChanged(object sender, System.EventArgs e)
    {
        UpdatePlayer();


        Debug.Log("ultiplayerManager_OnPlayerDataNetworkListChanged called from the Character Select Player script");
    }

    private void UpdatePlayer()
    {
        if (MultiplayerManager.Instance.IsPlayerIndexConnected(playerIndex))
        {
            Show();

            //Get the clientId to check who is ready
            PlayerData playerData = MultiplayerManager.Instance.GetPlayerDataFromPlayerIndex(playerIndex);
            readyGameObject.SetActive(CharacterSelectReady.Instance.IsPlayerReady(playerData.clientId));

            playerNameText.text = playerData.playerName.ToString();

            playerLight.SetActive(CharacterSelectReady.Instance.IsPlayerReady(playerData.clientId));
        }
        else
        {
            Hide();
        }
    }


    private void Show()
    {
        gameObject.SetActive(true);

    }
    private void Hide()
    {
        gameObject.SetActive(false);
    }
}
