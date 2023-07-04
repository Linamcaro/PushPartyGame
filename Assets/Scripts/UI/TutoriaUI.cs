
using UnityEngine;
using UnityEngine.UI;
using UnityEngine;
using System;

public class TutoriaUI : MonoBehaviour
{
    [SerializeField] private Button readyButton;

    private void Awake()
    {

        readyButton.onClick.AddListener(() =>
        {
            PushPartyGameManager.Instance.OnStartButtonPressed();

        });
    }


    private void Start()
    {
        PushPartyGameManager.Instance.OnLocalPlayerReadyChanged += PushPartyGameManager_OnLocalPlayerReadyChanged;
        Show();
    }

    private void PushPartyGameManager_OnLocalPlayerReadyChanged(object sender, EventArgs e)
    {
        if (PushPartyGameManager.Instance.IsLocalPlayerReady())
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
