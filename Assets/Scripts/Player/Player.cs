using UnityEngine;
using Unity.Netcode;
using Cinemachine;
using UnityEngine.InputSystem;
using Unity.Netcode.Components;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.UI;

public class Player : NetworkBehaviour
{

    [SerializeField] private CinemachineFreeLook cmCamera;
    [SerializeField] float speed = 5f;
    [SerializeField] float jumpForce = 5f;
    [SerializeField] float fallMultiplier = 2.5f;
    [SerializeField] float lowJumpMultiplier = 2f;

    private NetworkAnimator animator;
    Vector3 movement;
    bool fireButton;
    bool isJumping = false;
    private Rigidbody rb;


    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();

        if (IsOwner)
        {
            rb = GetComponent<Rigidbody>();
            animator = GetComponent<NetworkAnimator>();
            cmCamera.Priority = 100;
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


    private void FixedUpdate()
    {
        if (!IsOwner || !Application.isFocused) return;

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

}
