using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
public class SpeedBar : MonoBehaviour
{
    // Start is called before the first frame update

    private Slider speedSlider;
    public Gradient gradient;
    private Image fill;
    private float currentspeed;
    private float maxSpeed = 1;
    private float decreaseInterval = 0f;

    void Start()
    {
        PlayerMovement.PlayerMovementInstance.OnCallSpeed += PlayerMovement_OnCallSpeed;
        speedSlider = GameObject.FindWithTag("Slider").GetComponent<Slider>();
        fill = GameObject.FindWithTag("Fill").GetComponent<Image>();
        speedSlider.gameObject.SetActive(false);
    }
    private void  PlayerMovement_OnCallSpeed(object sender, EventArgs e)
    {

        StartCoroutine(DecreaseSliderOverTime());
    }




    public IEnumerator DecreaseSliderOverTime()
    {

        currentspeed = maxSpeed;

        speedSlider.maxValue = maxSpeed;
        speedSlider.value = currentspeed;
        speedSlider.gameObject.SetActive(true);
        fill.color = gradient.Evaluate(1f);
        while (currentspeed > 0f)
        {
            yield return new WaitForSeconds(decreaseInterval);

            currentspeed -= 0.001f;
            speedSlider.value = currentspeed;
            fill.color = gradient.Evaluate(speedSlider.normalizedValue);
        }
        
        speedSlider.gameObject.SetActive(false);
       
    }
}
