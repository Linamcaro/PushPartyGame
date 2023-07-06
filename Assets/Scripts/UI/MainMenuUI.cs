using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class MainMenuUI : MonoBehaviour
{

   
    public void PlayButton()
    {
        //SoundManager.Instance.PlayerMenuSound();
        LoadScenes.ChangeScene(LoadScenes.Scene.Lobby);
        Debug.Log("Main Scene called");
    }

    public void QuitButton()
    {
        
        LoadScenes.QuitGame();
        Debug.Log("Main Scene called");
    }


    /*public void MenuSound()
    {
        SoundManager.Instance.PlayerMenuSound();
    }*/

}
