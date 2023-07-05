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
        foreach (ContactPoint contact in collision.contacts)
        {
            if (collision.gameObject.tag == "Player")
            {
                hitDir = contact.normal;
                PlayerMovement collisionPM = collision.gameObject.GetComponent<PlayerMovement>();

                if (collisionPM != myPlayerMovement && myPlayerMovement.isAttacking)
                {
                    collisionPM.HitPlayer(-hitDir * force, stunTime);
                }
                return;
            }
        }
    }

}
