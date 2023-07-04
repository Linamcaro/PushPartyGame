using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class OptionsUI : MonoBehaviour
{
    private static OptionsUI _instance;

    public static OptionsUI Instance
    {
        get
        {
            return _instance;
        }
    }

    [SerializeField] private GameObject OptionsMenuUI;


    void Start()
    {
        Hide();
    }

    

    public void SoundEffectsButton()
    {
        SoundManager.Instance.ChangeVolume();
    }

    public void MusicButton()
    {
        MusicManager.Instance.ChangeVolume();
    }

    public void CloseButton()
    {
        Hide();
    }

    public void QuitGameButton()
    {
        NetworkManager.Singleton.Shutdown();
        LoadScenes.ChangeScene(LoadScenes.Scene.MainMenu);
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
