using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using Unity.Netcode;

public class SpeedBar : NetworkBehaviour
{
    private Slider speedSlider;
    public Gradient gradient;
    private Image fill;
    private float currentspeed;
    private float maxSpeed = 1;
    private float decreaseInterval = 0f;

    private Camera controlledCamera;
    private float originalFOV;
    private float maxFOV = 90f; 

    void Start()
    {
        if (!IsOwner) return;

        PlayerMovement.PlayerMovementInstance.OnCallSpeed += PlayerMovement_OnCallSpeed;
        speedSlider = GameObject.FindWithTag("Slider").GetComponent<Slider>();
        fill = GameObject.FindWithTag("Fill").GetComponent<Image>();
        speedSlider.gameObject.SetActive(false);

        
        controlledCamera = Camera.main; 
        originalFOV = controlledCamera.fieldOfView;
    }

    private void PlayerMovement_OnCallSpeed(object sender, EventArgs e)
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

        
        controlledCamera.fieldOfView = maxFOV;

        while (currentspeed > 0f)
        {
            yield return new WaitForSeconds(decreaseInterval);
            currentspeed -= 0.001f;
            speedSlider.value = currentspeed;
            fill.color = gradient.Evaluate(speedSlider.normalizedValue);

           
            controlledCamera.fieldOfView = Mathf.Lerp(originalFOV, maxFOV, currentspeed / maxSpeed);
        }

        
        controlledCamera.fieldOfView = originalFOV;
        speedSlider.gameObject.SetActive(false);
    }
}
