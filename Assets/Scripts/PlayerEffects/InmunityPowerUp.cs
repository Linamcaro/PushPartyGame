using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InmunityPowerUp : MonoBehaviour
{
    public GameObject inmunityEffect;
    private float multiplier = 1.4f;
    private float inmunityDuration = 1f;

    private bool isImmune = false;
    private Collider[] colliders;
    private float immunityDuration = 10f;

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
            Destroy(gameObject);
        }
    }


}
