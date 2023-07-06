using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LobbyCreateUI : MonoBehaviour
{


    [SerializeField] TMP_InputField lobbyNameInputField;

    public void createPublicButton()
    {
        GameLobby.Instance.CreateLobby(lobbyNameInputField.text, false);
    }

    public void createPrivateButton()
    {
        GameLobby.Instance.CreateLobby(lobbyNameInputField.text, true);
    }

    private void Awake()
    {
        
    }

}
