using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class CharacterSelectUI : MonoBehaviour
{

    public void MainMenuButton()
    {
        NetworkManager.Singleton.Shutdown();
        LoadScenes.ChangeScene(LoadScenes.Scene.Lobby);
    }

    public void ReadyButton()
    {
        CharacterSelectReady.Instance.PlayerReady();
    }


}
