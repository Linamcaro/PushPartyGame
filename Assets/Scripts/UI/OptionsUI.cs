using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using UnityEngine.UI;

public class OptionsUI : MonoBehaviour
{

    [SerializeField] private GameObject OptionsMenuUI;
    [SerializeField] private Button QuitGameButton;


    void Awake()
    {
        QuitGameButton.onClick.AddListener(() =>
        {
            NetworkManager.Singleton.Shutdown();
            LoadScenes.ChangeScene(LoadScenes.Scene.MainMenu);
        });
    }

        void Start()
    {
        Hide();
    }

   

    public void CloseButton()
    {
        Hide();
    }

    

    private void Show()
    {
        OptionsMenuUI.SetActive(true);

    }
    private void Hide()
    {
        OptionsMenuUI.SetActive(false);
    }
}
