/*using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LivesManager : MonoBehaviour
{
    public TMPro.TextMeshProUGUI livesText;
    public static LivesManager Instance;

    public void Awake()
    {
        if (Instance != null)
        {
            Destroy(this.gameObject);
        }
        else
        {
            Instance = this;
        }
    }

    public void ChangeLives(int lives)
    {
        livesText.text = lives.ToString();
    }

}*/
