using System.Collections;
using System.Collections.Generic;
using TMPro;
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

    }
}