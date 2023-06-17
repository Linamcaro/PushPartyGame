using UnityEngine;
using Unity.Netcode;

public class PlayerController : NetworkBehaviour
{
    [SerializeField] float speed;
    [SerializeField] bool isOnGround = true;
    [SerializeField] float jumpForce;
    [SerializeField] float gravityModifier;

    Rigidbody playerRb;
    private Transform cameraTransform;

    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();
        playerRb = GetComponent<Rigidbody>();
        Physics.gravity *= gravityModifier;
        cameraTransform = Camera.main.transform;
    }

    void Update()
    {
        Movement(speed);

    }

    /// <summary>
    /// move the player to the front,back, and sides at a certain speed
    /// </summary>
    /// <param name="speed"></param>
    public void Movement(float speed)
    {
        if (!IsOwner || !Application.isFocused) return;

        float verticalInput = Input.GetAxis("Vertical");
        float horizontalInput = Input.GetAxis("Horizontal");

        Vector3 move = new Vector3(horizontalInput, 0f, verticalInput);

        move = cameraTransform.forward * move.z + cameraTransform.right * move.x;
        move.y = 0;

        // Calculate the movement direction relative to the camera's rotation
        Vector3 movementDirection = new Vector3(horizontalInput, 0f, verticalInput);

        // Preserve the Y component of the player's velocity
        playerRb.velocity = new Vector3(movementDirection.x * speed, playerRb.velocity.y, movementDirection.z * speed);
        if (Input.GetButtonDown("Jump") && isOnGround)
        {
            playerRb.velocity = new Vector3(playerRb.velocity.x, jumpForce, playerRb.velocity.y);
            isOnGround = false;

        }

    }


    //check if player is standing on ground
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isOnGround = true; 
        }

        if(collision.gameObject.CompareTag("Player"))
        {

        }
    }

}