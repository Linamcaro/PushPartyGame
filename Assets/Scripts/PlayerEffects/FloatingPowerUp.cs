using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloatingPowerUp : MonoBehaviour
{
    private Transform visual;
    private Vector3 initialPosition; // Variable para almacenar la posici�n inicial

    private void Start()
    {
        visual = transform;
        initialPosition = visual.position; // Almacenar la posici�n inicial
        StartCoroutine(PickUpFloatingAnimation());
    }

    private IEnumerator PickUpFloatingAnimation()
    {
        while (true)
        {
            visual.Rotate(Vector3.up, 60 * Time.deltaTime, Space.World);
            visual.position = initialPosition + new Vector3(0, Mathf.Sin(Time.time) * 0.5f, 0f); // Calcular la posici�n final
            yield return null;
        }
    }
}

