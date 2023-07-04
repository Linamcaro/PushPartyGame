
using System;
using UnityEngine;



public class WaitingForPlayers : MonoBehaviour
{


  
    private void Start()
    {
        PushPartyGameManager.Instance.OnLocalPlayerReadyChanged += PushPartyGameManager_OnLocalPlayerReadyChanged;
        PushPartyGameManager.Instance.OnStateChanged += PushPartyGameManager_OnStateChanged;
        Hide();
    }


    /// <summary>
    /// Check if the Count Down To Start is active then hide the UI
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void PushPartyGameManager_OnStateChanged(object sender, System.EventArgs e)
    {
        if(PushPartyGameManager.Instance.IsCountdownToStartActive())
        {
            Hide();
        }
    }

    private void PushPartyGameManager_OnLocalPlayerReadyChanged(object sender, EventArgs e)
    {
        if(PushPartyGameManager.Instance.IsLocalPlayerReady()) {
            Show();
        }
    }


    //Hide UI
    private void Show()
    {
        gameObject.SetActive(true);
    }

    //Hide UI
    private void Hide()
    {
        gameObject.SetActive(false);
    }

}
