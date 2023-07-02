using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI playerLives;

    private void Start()
    {

        PushPartyGameManager.Instance.OnStateChanged += PushPartyGameManager_OnStateChanged;

        PlayerSpawn.Instance.OnLivesChanged += PlayerRespawn_OnLivesChanged;
        Hide();
    }

    private void PlayerRespawn_OnLivesChanged(object sender, EventArgs e)
    {
        UpdateUI();
    }

    private void PushPartyGameManager_OnStateChanged(object sender, EventArgs e)
    {
        if (PushPartyGameManager.Instance.IsGamePlaying())
        {
            UpdateUI();
            Show();

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

    private void UpdateUI()
    {
        playerLives.text = PlayerSpawn.Instance.GetPlayerLives().ToString();
    }

}
