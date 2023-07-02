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
            PickUp(other);
        }
    }
    void PickUp(Collider player)
    {

        Instantiate(lifeEffect, transform.position, transform.rotation);
        PlayerRespawn stats = player.GetComponent<PlayerRespawn>();
        stats.lives += 1;
        Destroy(gameObject);
    }

}