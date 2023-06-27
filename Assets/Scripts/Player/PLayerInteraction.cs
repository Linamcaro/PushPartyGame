using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PLayerInteraction : MonoBehaviour
{
    private float pushForce = 7f;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("PLayer"))
        {
            Vector3 forceDirection = collision.transform.position - transform.position;
            forceDirection.Normalize();

            Rigidbody playerRb = collision.gameObject.GetComponent<Rigidbody>();
            playerRb.AddForce(forceDirection * pushForce, ForceMode.Impulse);
        }

    }
}