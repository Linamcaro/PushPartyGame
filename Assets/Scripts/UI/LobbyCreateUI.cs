using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LobbyCreateUI : MonoBehaviour
{

    [SerializeField] private Button closeButton;
    [SerializeField] private Button createPublicButton;
    [SerializeField] private Button createPrivateButton;
    [SerializeField] private TMP_InputField lobbyNameInputField;
    private int randomLobbyNumber;
    private string lobbyName;


    private void Awake()
    {
        createPublicButton.onClick.AddListener(() => {
            SetLobbyName();
            GameLobby.Instance.CreateLobby(lobbyName, false);
        });
        createPrivateButton.onClick.AddListener(() => {
            SetLobbyName();
            GameLobby.Instance.CreateLobby(lobbyName, true);
        });
        closeButton.onClick.AddListener(() => {
            Hide();
        });
    }

    private void Start()
    {
        Hide();
    }


    public void SetLobbyName()
    {
        if (lobbyNameInputField.text == "")
        {
            randomLobbyNumber = Random.Range(0, 1000);
            lobbyName = "Lobby" + randomLobbyNumber;
        }
        else
        {
            lobbyName = lobbyNameInputField.text;
        }
    }

    public void Show()
    {
        gameObject.SetActive(true);

        createPublicButton.Select();
    }

    private void Hide()
    {
        gameObject.SetActive(false);
    }
}
