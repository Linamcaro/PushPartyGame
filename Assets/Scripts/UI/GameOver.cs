using System;
using UnityEngine;

public class GameOver : MonoBehaviour
{

    private void Start()
    {
        PushPartyGameManager.Instance.OnStateChanged += PushPartyGameManager_OnStateChanged;
        Hide();

    }

    private void PushPartyGameManager_OnStateChanged(object sender, EventArgs e)
    {
        if (PushPartyGameManager.Instance.IsCountdownToStartActive())
        {
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
}
