using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeScenes : MonoBehaviour
{
    public void MainMenu()
    {
        LoadScenes.LoadTagetScene(LoadScenes.Scene.Lobby);
    }

    public void Quit()
    {
        LoadScenes.QuitGame();
    }
}
