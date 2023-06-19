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

    private void OnMove(InputValue value)
    {
        if (!IsOwner || !Application.isFocused) return;

        movement = value.Get<Vector3>();
        if (Keyboard.current.sKey.wasPressedThisFrame)
        {

            animator.SetTrigger("Slide");
        }

    }

    private void OnFire1()
    {
        if (!IsOwner || !Application.isFocused) return;
        animator.SetTrigger("Attack1");
    }

    private void OnFire2()
    {
        if (!IsOwner || !Application.isFocused) return;
        animator.SetTrigger("Attack2");
    }

     
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
        // Se aplica un movimiento horizontal para que pueda caminar
        rb.velocity = new Vector3(movement.x * speed, rb.velocity.y, movement.z * speed);

        // Cuando cae se le aplica gravedad personalizada para una fuerza adicional hacia arriba para que suavice la caida
        if (rb.velocity.y < 0)
        {
            rb.velocity += Vector3.up * Physics.gravity.y * (fallMultiplier - 1) * Time.deltaTime;
        }
        //Cuando el personaje sube después de saltar se le aplica una fuerza adicional con un suavizado 
        else if (rb.velocity.y > 0 && !isJumping)
        {
            rb.velocity += Vector3.up * Physics.gravity.y * (lowJumpMultiplier - 1) * Time.deltaTime;
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
