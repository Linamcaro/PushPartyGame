
using UnityEngine;


public class TestLobbyUI : MonoBehaviour
{

    public void StartHost()
    {
        MultiplayerManager.Instance.StartHost();
        LoadScenes.LoadTagetScene(LoadScenes.Scene.CharacterSelection);
    }

    public void StartClient()
    {
        MultiplayerManager.Instance.StartClient(); 
        
    }

    
    

}
