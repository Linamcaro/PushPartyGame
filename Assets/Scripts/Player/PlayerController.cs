using UnityEngine;
using Unity.Netcode;
using Cinemachine;

public class PlayerController : NetworkBehaviour
{
    [SerializeField] private CinemachineVirtualCamera cmCamera;
    [SerializeField] float speed;
    [SerializeField] bool isOnGround = true;
    [SerializeField] float jumpForce;
    [SerializeField] float gravityModifier;
    [SerializeField] private Vector3 currentPosition;
    [SerializeField] private Vector3 currentRotation;

    Rigidbody playerRb;
    private Transform cameraTransform;


    private void Awake()
    {
        
        Physics.gravity *= gravityModifier;
        cameraTransform = Camera.main.transform;
    }

    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();

        if (!IsOwner)
        {
            
        }
        else
        {
            playerRb = GetComponent<Rigidbody>();
            cmCamera.Priority = 100;
        }

       
    }

    void FixedUpdate()
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
        bool boostInput = Input.GetButtonDown("Fire1");

        Vector3 move = new Vector3(horizontalInput, 0f, verticalInput);

        // Bloquear el movimiento hacia atrás
        if (move.z < 0f)
        {
            move.z = 0f; 
        }

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
    }


    /*  if (boostInput)
    {
        Vector3 boostDirection = transform.forward;
        playerRb.AddForce(boostDirection * boostForce, ForceMode.Impulse);
    }

    // Impulso del personaje
    if (boostInput)
    {
        Vector3 boostDirection = transform.forward;
        playerRb.AddForce(boostDirection * boostForce, ForceMode.Impulse);
    }*/

}