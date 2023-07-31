using System.Collections;
using System.Collections.Generic;
using Netcode.Extensions;
using Unity.Netcode;
using UnityEngine;

public class InmunityPowerUp : NetworkBehaviour
{
    public GameObject inmunityEffect;
    [SerializeField] private PowerUpHelper PowerUpHelper;

    private Collider[] colliders;


    private void Start()
    {

        colliders = GetComponentsInChildren<Collider>();
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {

            PlayerSpawn.Instance.CallInmunity(other);

            //  PlayerSpawn stats =  other.GetComponent<PlayerRespawn>();
            //stats.CallInmunity(other);
            SpeedBar speedBarStats = other.GetComponent<SpeedBar>();
            speedBarStats.StartCoroutine(speedBarStats.DecreaseSliderOverTime());


            NetworkObjectPool.Singleton.ReturnNetworkObject(this.NetworkObject, PowerUpHelper.prefab);
           
            
        }
    }


}
