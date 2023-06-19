using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Cinemachine;

public class Player : MonoBehaviour
{
    [SerializeField] float speed = 5f;
    [SerializeField] float jumpForce = 5f;
    [SerializeField] float fallMultiplier = 2.5f;
    [SerializeField] float lowJumpMultiplier = 2f;

    public Transform player;
    public Transform orientation;
    private Animator animator;
    Vector3 movement;
    bool isJumping = false;
    private Rigidbody rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
    }

    private void OnMove(InputValue value)
    {
        movement = value.Get<Vector3>();
        if (Keyboard.current.sKey.wasPressedThisFrame)
        {

            animator.SetTrigger("Slide");
        }
        if (Keyboard.current.qKey.wasPressedThisFrame)
        {

            animator.SetTrigger("Attack1");
        }

        if (Keyboard.current.eKey.wasPressedThisFrame)
        {

            animator.SetTrigger("Attack2");
        }
    }

    private void OnJump(InputValue value)
    {
        if (value.isPressed && !isJumping)
        {
            //animator.SetBool("Jump", true);
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            isJumping = true;


        }

    }

    private void FixedUpdate()
    {
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
