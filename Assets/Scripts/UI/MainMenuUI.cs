using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class MainMenuUI : MonoBehaviour
{
    public void PlayButton()
    {
       
        LoadScenes.ChangeScene(LoadScenes.Scene.Lobby);
        Debug.Log("Main Scene called");
    }

    public void QuitButton()
    {
      
        LoadScenes.QuitGame();
        Debug.Log("Main Scene called");
    }

}
