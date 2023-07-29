
using UnityEngine;


public class TestLobbyUI : MonoBehaviour
{
    private float volume = 1;
    public void StartHost()
    {
        MultiplayerManager.Instance.StartHost();
        LoadScenes.LoadTagetScene(LoadScenes.Scene.CharacterSelection);
    }

    public void StartClient()
    {
        MultiplayerManager.Instance.StartClient(); 
        
    }

    public void ClickButtonSound()
    {
        SoundManager.Instance.PlayMenuSound();
    }


}
