using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class LoadScenes
{
    //scene names
    public enum Scene
    {
        MainMenu,
        Lobby,
        MainScene,
        Loading,
        GameOver,
        Winner,
        Lina,
        Lobby2,
    }

    //method to load a scene
    public static void LoadTagetScene(Scene targetScene)
    {
        NetworkManager.Singleton.SceneManager.LoadScene(targetScene.ToString(), LoadSceneMode.Single); 
    }

    //method to quit the game
    public static void QuitGame()
    {
        Application.Quit();
    }

}
