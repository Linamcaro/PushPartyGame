using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LivePowerUp : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject lifeEffect;
    private float multiplier = 1.4f;

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
        PlayerRespawn.Instance.IncreasePlayerLives();
        Destroy(gameObject);
    }

}
