using Unity.Netcode;
using UnityEngine;

public class PlayersCollision : NetworkBehaviour
{
    [SerializeField] private float pushForce = 10f;

    private void OnCollisionEnter(Collision collision)
    {
        if (!IsOwner) return; // Only the owner player should initiate the push
        if (!NetworkManager.Singleton.IsServer) return;

        if (collision.gameObject.CompareTag("Player"))
        {
            // Calculate the direction of the push
            Vector3 pushDirection = collision.contacts[0].point - transform.position;
            pushDirection.y = 0f; // Ignore the vertical component to avoid lifting the other player

            // Normalize the push direction and apply the push force
            pushDirection.Normalize();
            Rigidbody otherPlayerRb = collision.gameObject.GetComponent<Rigidbody>();
            otherPlayerRb.AddForce(pushDirection * pushForce, ForceMode.Impulse);
        }
    }
}