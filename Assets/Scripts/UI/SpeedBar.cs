using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class SpeedBar : MonoBehaviour
{
    private Slider speedSlider;
    public Gradient gradient;
    private Image fill;
    private float currentspeed;
    private float maxSpeed = 1;
    private float decreaseInterval = 0f;

    private Camera controlledCamera;
    private float originalFOV;
    private float maxFOV = 90f; // Valor máximo del FOV cuando el slider esté en su valor máximo

    void Start()
    {
        PlayerMovement.PlayerMovementInstance.OnCallSpeed += PlayerMovement_OnCallSpeed;
        speedSlider = GameObject.FindWithTag("Slider").GetComponent<Slider>();
        fill = GameObject.FindWithTag("Fill").GetComponent<Image>();
        speedSlider.gameObject.SetActive(false);

        // Buscar la cámara por tag y guardar el FOV original
        controlledCamera = Camera.main; // Si es "Main Camera" en la jerarquía
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

        // Aumentar el FOV al valor máximo cuando se active el slider
        controlledCamera.fieldOfView = maxFOV;

        while (currentspeed > 0f)
        {
            yield return new WaitForSeconds(decreaseInterval);
            currentspeed -= 0.001f;
            speedSlider.value = currentspeed;
            fill.color = gradient.Evaluate(speedSlider.normalizedValue);

            // Actualizar el FOV mientras cambia el valor del slider
            controlledCamera.fieldOfView = Mathf.Lerp(originalFOV, maxFOV, currentspeed / maxSpeed);
        }

        // Restaurar el FOV original al desactivar el slider
        controlledCamera.fieldOfView = originalFOV;
        speedSlider.gameObject.SetActive(false);
    }
}
