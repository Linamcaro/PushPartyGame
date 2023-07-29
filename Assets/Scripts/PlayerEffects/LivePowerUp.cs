using System.Collections;
using System.Collections.Generic;
using Netcode.Extensions;
using Unity.Netcode;
using UnityEngine;

public class LivePowerUp : NetworkBehaviour
{
    [SerializeField] private PowerUpHelper PowerUpHelper;
    public GameObject lifeEffect;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PickUp();
        }
    }
    void PickUp()
    {
        Instantiate(lifeEffect, transform.position, transform.rotation);
        PlayerSpawn.Instance.IncreasePlayerLives();

        NetworkObjectPool.Singleton.ReturnNetworkObject(NetworkObject, PowerUpHelper.prefab);
    }

}
