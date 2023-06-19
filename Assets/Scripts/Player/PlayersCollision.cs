/*using Unity.Netcode;
using UnityEngine;


public class PlayersCollision : NetworkBehaviour
{

    public float collisionForce = 10f;

    private void OnCollisionEnter(Collision collision)
    {
        if (!IsServer) return;

        if (collision.gameObject.CompareTag("Player"))
        {
            // Calculate the direction from the other player to this player
            Vector3 forceDirection = transform.position - collision.gameObject.transform.position;
            forceDirection.Normalize();

            // Apply a force to throw the other player away
            Rigidbody otherRb = collision.gameObject.GetComponent<Rigidbody>();
            ApplyForceOnClientRpc(otherRb.gameObject.GetComponent<NetworkObject>(), forceDirection * collisionForce);
        }
    }

    [ClientRpc]
    private void ApplyForceOnClientRpc(NetworkObject otherPlayer, Vector3 force)
    {
        otherPlayer.GetComponent<Rigidbody>().AddForce(force, ForceMode.Impulse);
    }
}*/
