using UnityEngine;
using Unity.Netcode;
using Cinemachine;
using UnityEngine.InputSystem;
using Unity.Netcode.Components;


public class Player : NetworkBehaviour
{

    //movement
    [SerializeField] private float speed = 5f;
    [SerializeField] private float jumpForce = 5f;
    [SerializeField] private float fallMultiplier = 2.5f;
    [SerializeField] private float lowJumpMultiplier = 2f;

    //lives
    /*private float deathPointY;
    [SerializeField] private int lives;*/

    //position
    private Vector3 respawnPosition;
    private Vector3 movement;

    //Components
    private NetworkAnimator animator;
    private Rigidbody rb;
    [SerializeField] private CinemachineFreeLook cmCamera;

    //helper    
    private bool isJumping = false;
    


    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();

        if (IsOwner)
        {
            rb = GetComponent<Rigidbody>();
            animator = GetComponent<NetworkAnimator>();
            cmCamera.Priority = 100;
          //  lives = 2;
         //   deathPointY = -15f;

        }


    }

    /// <summary>
    /// called when player hit the movement keys
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

    }

    /// <summary>
    /// called when player left click
    /// </summary>
    private void OnFire1()
    {
        if (!IsOwner || !Application.isFocused) return;
        animator.SetTrigger("Attack1");
    }

    /// <summary>
    /// called when player right click
    /// </summary>
    private void OnFire2()
    {
        if (!IsOwner || !Application.isFocused) return;
        animator.SetTrigger("Attack2");
    }

    /// <summary>
    /// Called when player hit the jump key
    /// </summary>
    private void OnJump()
    {
        if (!IsOwner || !Application.isFocused) return;
        if (!isJumping)
        {
            //animator.SetBool("Jump", true);
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            isJumping = true;

        }

    }


    /*private void Update()
    {
        RespawnPlayer();
    }*/

    private void FixedUpdate()
    {
        if (!IsOwner || !Application.isFocused) return;

        MovePlayerClient();

    }

    /*[ServerRpc]
    private void MovePlayerServerRpc()
    {
        rb.velocity = new Vector3(movement.x * speed, rb.velocity.y, movement.z * speed);

        // When it falls custom gravity is applied to the character for additional upward force to soften the fall
        if (rb.velocity.y < 0)
        {
            rb.velocity += Vector3.up * Physics.gravity.y * (fallMultiplier - 1) * Time.deltaTime;
        }
        //Check if the player can jump and apply the jump force
        else if (rb.velocity.y > 0 && !isJumping)
        {
            rb.velocity += Vector3.up * Physics.gravity.y * (lowJumpMultiplier - 1) * Time.deltaTime;
        }
    }*/



    //Client Authorative Movement
    private void MovePlayerClient()
    {
        //horizontal movement
        rb.velocity = new Vector3(movement.x * speed, rb.velocity.y, movement.z * speed);

        // When it falls custom gravity is applied to the character for additional upward force to soften the fall
        if (rb.velocity.y < 0)
        {
            rb.velocity += Vector3.up * Physics.gravity.y * (fallMultiplier - 1) * Time.deltaTime;
        }
        //Check if the player can jump and apply the jump force
        else if (rb.velocity.y > 0 && !isJumping)
        {
            rb.velocity += Vector3.up * Physics.gravity.y * (lowJumpMultiplier - 1) * Time.deltaTime;
        }
    }


    /// <summary>
    /// Check if player hit the ground
    /// </summary>
    /// <param name="collision"></param>
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isJumping = false;
            //animator.SetBool("Jump", false);


        }
    }

    
   /* private void RespawnPlayer()
    {

        if (transform.position.y < deathPointY)
        {
            lives--;

            if (lives > 0)
            {

                respawnPosition = new Vector3(0, 1f, transform.position.z);
                //Move player to the respawn position
                transform.position = respawnPosition;
            } else
            {
               
                Debug.Log("Game Over");

            }
        }
        
    }*/

}
