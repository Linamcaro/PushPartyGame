using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Unity.Netcode;

public class GameOver : MonoBehaviour
{

    [SerializeField] private GameObject WinnerUI;
    [SerializeField] private Button playAgainWinner;
    [SerializeField] private Button playAgainLoser;
    [SerializeField] private GameObject GameOverUI;

    void Awake()
    {
        playAgainWinner.onClick.AddListener(() =>
        {
            NetworkManager.Singleton.Shutdown();
            LoadScenes.ChangeScene(LoadScenes.Scene.MainMenu);
        });

        playAgainLoser.onClick.AddListener(() =>
        {
            NetworkManager.Singleton.Shutdown();
            LoadScenes.ChangeScene(LoadScenes.Scene.MainMenu);
        });
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
                ShowWinner();
                

            }
            else
            {
                ShowLoser();
                
            }
        }
    }

    private void ShowLoser()
    {
        GameOverUI.SetActive(true);
    }

    private void HideLoser()
    {
        GameOverUI.SetActive(false);
    }

    private void ShowWinner()
    {
        WinnerUI.SetActive(true);
    }

    private void HideWinner()
    {
        WinnerUI.SetActive(false);
    }
}
