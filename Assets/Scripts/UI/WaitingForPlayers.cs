using System.Collections;
using System.Collections.Generic;
using UnityEditor.Search;
using UnityEngine;
using UnityEngine.UI;
using Unity.Netcode;

public class WaitingForPlayers : MonoBehaviour
{

    [SerializeField] private Button readyButton;
    [SerializeField] private GameObject waitingForPlayersScreen;


    private void Awake()
    {
        Hide();

        readyButton.onClick.AddListener(() =>
        {
            PushPartyGameManager.Instance.OnStartButtonPressed();
            OnLocalPlayerReadyChanged();

        });
    }



    private void Start()
    {
        PushPartyGameManager.Instance.OnStateChanged += PushPartyGameManager_OnStateChanged;
        
    }

    private void OnLocalPlayerReadyChanged()
    {
        if(PushPartyGameManager.Instance.IsLocalPlayerReady()) {
            Show();
        }
    }


    /// <summary>
    /// Check if the Count Down To Start is active then hide the UI
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void PushPartyGameManager_OnStateChanged(object sender, System.EventArgs e)
    {
        if (PushPartyGameManager.Instance.IsCountdownToStartActive())
        {
            Hide();
        }
    }
    //Hide UI
    private void Show()
    {
        waitingForPlayersScreen.SetActive(true);
    }

    //Hide UI
    private void Hide()
    {
        waitingForPlayersScreen.SetActive(false);
    }

}
