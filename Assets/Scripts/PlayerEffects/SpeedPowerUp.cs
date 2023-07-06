using System.Collections;
using System.Collections.Generic;
using Netcode.Extensions;
using Unity.Netcode;
using UnityEngine;

public class SpeedPowerUp : NetworkBehaviour
{
    public GameObject speedEffect;
    private float multiplier = 1.4f;
    private float duration = 10f;
    private Collider[] colliders;
    private void Start()
    {

        colliders = GetComponentsInChildren<Collider>();
    }
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Instantiate(speedEffect, transform.position, transform.rotation);
            PlayerMovement stats = other.GetComponent<PlayerMovement>();
            stats.CallSpeed(other);
            NetworkObjectPool.Singleton.ReturnNetworkObject(this.NetworkObject, this.gameObject);
        }
    }

}
