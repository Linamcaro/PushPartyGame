using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Unity.Netcode;

public class GameOver : MonoBehaviour
{

    [SerializeField] private TextMeshProUGUI GameOverText;
    [SerializeField] private Button  playAgain;
    [SerializeField] private GameObject GameOverUI;

    void Awake()
    {
        playAgain.onClick.AddListener(() =>
        {
            NetworkManager.Singleton.Shutdown();
            LoadScenes.ChangeScene(LoadScenes.Scene.Lobby);
        });

        Hide();
    }
    void Start()
    {
        PushPartyGameManager.Instance.OnStateChanged += PushPartyGameManager_OnStateChanged;
    }

    private void PushPartyGameManager_OnStateChanged(object sender, EventArgs e)
    {
        if (PushPartyGameManager.Instance.IsGameOver())
        {
            
            if(PlayerSpawn.Instance.GetPlayerLives() > 0)
            {
                Show();
                GameOverText.text = "Winner";
                
            }
            else
            {
                Show();
                GameOverText.text = "Looser";
            }
        }
    }

    private void Show()
    {
        GameOverUI.SetActive(true);
    }

    private void Hide()
    {
        GameOverUI.SetActive(false);
    }
}
