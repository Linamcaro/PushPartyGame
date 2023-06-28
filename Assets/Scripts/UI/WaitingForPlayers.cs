using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaitingForPlayers : MonoBehaviour
{

    private void Start()
    {
        PushPartyGameManager.Instance.OnStateChanged += PushPartyGameManager_OnStateChanged;
        
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
    private void Hide()
    {
        gameObject.SetActive(false);
    }

}
