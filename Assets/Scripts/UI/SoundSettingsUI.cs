using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SoundSettingsUI : MonoBehaviour
{
    [SerializeField] private Slider musicSlider;
    [SerializeField] private Slider soundslider;


    public void MusicVolume()
    {
        MusicManager.Instance.ChangeVolume(musicSlider.value);
    }

    public void SoundVolume()
    {
        SoundManager.Instance.ChangeVolume(musicSlider.value);
    }

}
