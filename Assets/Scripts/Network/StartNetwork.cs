using Unity.Netcode;
using UnityEngine;

public class StartNetwork : MonoBehaviour
{
    /*public void StartServer()
    {
        NetworkManager.Singleton.StartServer();   
    }*/
    
    public void StartClient()
    {
        MultiplayerManager.Instance.StartClient();

    }

    public void StartHost()
    {
        MultiplayerManager.Instance.StartHost();
    }

}
