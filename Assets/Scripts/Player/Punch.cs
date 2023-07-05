using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class Punch : NetworkBehaviour
{
    private float force = 20f; //Force 10000f
    private float stunTime = 0.5f;
    private Vector3 hitDir;
    public PlayerMovement myPlayerMovement;

    private void Start()
    {
        force = myPlayerMovement.attackForce;
        stunTime = myPlayerMovement.attackStunTime;
    }

    void OnCollisionEnter(Collision collision)
    {
        Debug.Log("Hey 1");
        foreach (ContactPoint contact in collision.contacts)
        {
            Debug.Log("you 2");


            if (collision.gameObject.tag == "Player")
            {
                Debug.Log("Listen 3");

                hitDir = contact.normal;
                PlayerMovement collisionPM = collision.gameObject.GetComponent<PlayerMovement>();

                if (collisionPM != myPlayerMovement && myPlayerMovement.isAttacking)
                {
                    Debug.Log("Push 4");

                    collisionPM.HitPlayer(-hitDir * force, stunTime);
                }
                
                return;
            }
        }
    }

    //void OnTriggerEnter(Collider other)
    //{
    //    if (other.gameObject.tag == "Player")
    //    {
    //        hitDir = (other.transform.position - transform.position).normalized;
    //        PlayerMovement collisionPM = other.gameObject.GetComponent<PlayerMovement>();

    //        if (collisionPM != myPlayerMovement)
    //        {
    //            collisionPM.HitPlayer(-hitDir * force, stunTime);
    //        }
    //        return;
    //    }
    //}
}
