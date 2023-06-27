
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadScenes : MonoBehaviour
{
    public void LoadMainScene()
    {
        SceneManager.LoadScene("Lina");
    }

    public void QuitGame()
    {
        Application.Quit();
    }

}
