using System.Collections;
using System.Collections.Generic;
using Netcode.Extensions;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;
public class SpeedPowerUp : NetworkBehaviour
{
    public GameObject speedEffect;
    private float multiplier = 1.4f;
    private Collider[] colliders;
    [SerializeField] private Slider speedSlider;
    public Gradient gradient;
    public Image fill;
    private Coroutine decreaseCoroutine;
    private float currentspeed;
    private float maxSpeed = 1;
    private float decreaseInterval = 0f;
    private void Start()
    {
       //
        
        gradient.Evaluate(1f);
        colliders = GetComponentsInChildren<Collider>();
        
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Instantiate(speedEffect, transform.position, transform.rotation);
            PlayerMovement stats = other.GetComponent<PlayerMovement>();
            SpeedBar speedBarStats = other.GetComponent<SpeedBar>();
            stats.CallSpeed(other);
            //speedBarStats.StartCoroutine(speedBarStats.DecreaseSliderOverTime());


            NetworkObjectPool.Singleton.ReturnNetworkObject(this.NetworkObject, this.gameObject);
        }
    }






}
