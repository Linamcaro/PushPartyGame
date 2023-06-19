using UnityEngine;
using Unity.Netcode;
using Cinemachine;
using UnityEngine.InputSystem;

public class PlayerController : NetworkBehaviour
{
    [SerializeField] private CinemachineFreeLook cmCamera;
    [SerializeField] float speed = 5f;
    [SerializeField] float jumpForce = 5f;
    [SerializeField] float fallMultiplier = 2.5f;
    [SerializeField] float lowJumpMultiplier = 2f;

    private Animator animator;
    Vector3 movement;
    bool isJumping = false;

    [SerializeField] private Vector3 currentPosition;
    [SerializeField] private Vector3 currentRotation;

    Rigidbody playerRb;
   
      public override void OnNetworkSpawn()
      {
        base.OnNetworkSpawn();

        if (IsOwner)
        {
            playerRb = GetComponent<Rigidbody>();
            animator = GetComponent<Animator>();
            cmCamera.Priority = 100;
        }
        
               
      }

    /// <summary>
    /// Handle input from user
    /// </summary>
    /// <param name="value"></param>
    private void OnMove(InputValue value)
    {
        if (!IsOwner || !Application.isFocused) return;

        movement = value.Get<Vector3>();
        if (Keyboard.current.sKey.wasPressedThisFrame)
        {

            animator.SetTrigger("Slide");
        }
        if (Mouse.current.leftButton.wasPressedThisFrame)
        {

            animator.SetTrigger("Attack1");
        }

        if (Keyboard.current.eKey.wasPressedThisFrame)
        {

            animator.SetTrigger("Attack2");
        }
    }

    private void FixedUpdate()
    {
        // Se aplica un movimiento horizontal para que pueda caminar
        playerRb.velocity = new Vector3(movement.x * speed, playerRb.velocity.y, movement.z * speed);

        // Cuando cae se le aplica gravedad personalizada para una fuerza adicional hacia arriba para que suavice la caida
        if (playerRb.velocity.y < 0)
        {
            playerRb.velocity += Vector3.up * Physics.gravity.y * (fallMultiplier - 1) * Time.deltaTime;
        }
        //Cuando el personaje sube después de saltar se le aplica una fuerza adicional con un suavizado 
        else if (playerRb.velocity.y > 0 && !isJumping)
        {
            playerRb.velocity += Vector3.up * Physics.gravity.y * (lowJumpMultiplier - 1) * Time.deltaTime;
        }
    }



    /// <summary>
    /// Handle Player jumping
    /// </summary>
    /// <param name="value"></param>
    private void OnJump(InputValue value)
    {
        if (value.isPressed && !isJumping)
        {
            //animator.SetBool("Jump", true);
            playerRb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            isJumping = true;


        }

    }


    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isJumping = false;
            //animator.SetBool("Jump", false);
        }
    }

}