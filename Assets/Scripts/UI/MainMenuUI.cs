using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuUI : MonoBehaviour
{
    public void PlayButton()
    {
        LoadScenes.ChangeScene(LoadScenes.Scene.Lobby);
        Debug.Log("Main Scene called");
    }
}
