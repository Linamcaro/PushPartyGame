using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using Cinemachine;


public class PlayerMovement : NetworkBehaviour
{
    private static PlayerMovement _playerMovementInstance;
    public static PlayerMovement PlayerMovementInstance
    {
        get
        {
            return _playerMovementInstance;
        }
    }

    [SerializeField] private List<Vector3> spawnPositions;

    //Player jumping
    [SerializeField] public float jumpHeight = 2f;
    [SerializeField] public float jumpForce = 70f;
    [SerializeField] public float maxFallSpeed = 20.0f;
    [SerializeField] public float rotateSpeed = 25f;
    [SerializeField] public float maxVelocityChange = 10.0f;
    [SerializeField] private float moveSpeed = 10.0f;
    [SerializeField] public float airVelocity = 8f;
    [SerializeField] public float gravity = 10.0f;

    
    [SerializeField] private CinemachineFreeLook cmCamera;
    private Rigidbody rigidBody;

    //Helper Variables
    private bool canMove = true; 
    private bool isStuned = false;
    private bool wasStuned = false;
    private float pushForce;
    private float distToGround;
    private Vector3 pushDir;



    private bool isJumping;
    private bool isWalking;
    private bool isSliding;
    private bool slide = false;

    public override void OnNetworkSpawn()
    {

        if (IsOwner)
        {
            _playerMovementInstance = this;
            cmCamera.Priority = 100;
            rigidBody = GetComponent<Rigidbody>();
            rigidBody.freezeRotation = true;
            rigidBody.useGravity = false;
            // get the distance to ground
            distToGround = GetComponent<Collider>().bounds.extents.y;
        }

        transform.position = spawnPositions[(int)OwnerClientId];

    }

    private void Update()
    {
        if (!IsOwner)
        {
            return;
        }
        RaycastHit hit;
        if (Physics.Raycast(transform.position, -Vector3.up, out hit, distToGround + 0.1f))
        {
            if (hit.transform.tag == "Slide")
            {
                slide = true;
            }
            else
            {
                slide = false;
            }
        }
    }


    private void FixedUpdate()
    {
        if (!IsOwner)
        {
            return;
        }

        HandleMovement();
    }

    /// <summary>
    /// Returns if player is walking
    /// </summary>
    /// <returns></returns>
    public bool IsWalking()
    {
        return isWalking;
    }

    /// <summary>
    /// Returns if player jumped
    /// </summary>
    /// <returns></returns>
    public bool IsJumping()
    {
        return isJumping;
    }

    public bool IsSliding()
    {
        return isSliding;
    }


    /// <summary>
    /// Move the player according to the user input 
    /// </summary>
    private void HandleMovement()
    {
        if (!PushPartyGameManager.Instance.IsGamePlaying())
        {
            return;
        }


        Vector2 inputMovement = PlayerController.Instance.GetPlayerMovement();
        bool jump = PlayerController.Instance.PlayerJumped();

        Vector3 moveDir = new Vector3(inputMovement.x, 0, inputMovement.y);

        //bool slide = PlayerController.Instance.PlayerSlide();

        if (canMove)
        {
            if (moveDir.x != 0 || moveDir.z != 0)
            {
                //Direction of the character
                Vector3 targetDir = moveDir;

                targetDir.y = 0;
                if (targetDir == Vector3.zero)
                    targetDir = transform.forward;

                //Rotation of the character to where it moves
                Quaternion tr = Quaternion.LookRotation(targetDir);

                //Rotate the character smoothly(little by little)
                Quaternion targetRotation = Quaternion.Slerp(transform.rotation, tr, Time.deltaTime * rotateSpeed);
                transform.rotation = targetRotation;
            }


            if (IsGrounded())
            {
                // Calculate how fast we should be moving
                Vector3 targetVelocity = moveDir;
                targetVelocity *= moveSpeed;

                // Apply a force that attempts to reach our target velocity
                Vector3 velocity = rigidBody.velocity;

                //If I'm slowing down the character
                if (targetVelocity.magnitude < velocity.magnitude) 
                {
                    targetVelocity = velocity;
                    rigidBody.velocity /= 1.1f;
                }
                Vector3 velocityChange = (targetVelocity - velocity);
                velocityChange.x = Mathf.Clamp(velocityChange.x, -maxVelocityChange, maxVelocityChange);
                velocityChange.z = Mathf.Clamp(velocityChange.z, -maxVelocityChange, maxVelocityChange);
                velocityChange.y = 0;

                if (!slide)
                {
                    if (Mathf.Abs(rigidBody.velocity.magnitude) < moveSpeed * 1.0f)
                        rigidBody.AddForce(velocityChange, ForceMode.VelocityChange);
                }
                else if (Mathf.Abs(rigidBody.velocity.magnitude) < moveSpeed * 1.0f)
                {
                    rigidBody.AddForce(moveDir * 0.15f, ForceMode.VelocityChange);
                    //Debug.Log(rb.velocity.magnitude);
                }

                // check if can jump and apply velocity
                if (IsGrounded() && jump)
                {
                    rigidBody.velocity = new Vector3(velocity.x, CalculateJumpVerticalSpeed(), velocity.z);
                    isJumping = true;
                }

            }
            else
            {
                if (!slide)
                {
                    Vector3 targetVelocity = new Vector3(moveDir.x * airVelocity, rigidBody.velocity.y, moveDir.z * airVelocity);
                    Vector3 velocity = rigidBody.velocity;
                    Vector3 velocityChange = (targetVelocity - velocity);
                    velocityChange.x = Mathf.Clamp(velocityChange.x, -maxVelocityChange, maxVelocityChange);
                    velocityChange.z = Mathf.Clamp(velocityChange.z, -maxVelocityChange, maxVelocityChange);
                    rigidBody.AddForce(velocityChange, ForceMode.VelocityChange);
                    if (velocity.y < -maxFallSpeed)
                        rigidBody.velocity = new Vector3(velocity.x, -maxFallSpeed, velocity.z);
                }
                else if (Mathf.Abs(rigidBody.velocity.magnitude) < moveSpeed * 1.0f)
                {
                    rigidBody.AddForce(moveDir * 0.15f, ForceMode.VelocityChange);
                    IsSliding();
                }
            }

        }
        else
        {
            rigidBody.velocity = pushDir * pushForce;
        }
        // We apply gravity manually for more tuning control
        rigidBody.AddForce(new Vector3(0, -gravity * rigidBody.mass, 0));
    }

    /// <summary>
    /// From the jump height and gravity we deduce the upwards speed 
    /// for the character to reach at the apex.
    /// </summary>
    /// <returns></returns>
    float CalculateJumpVerticalSpeed()
    {

        return Mathf.Sqrt(2 * jumpHeight * gravity);
    }

    /// <summary>
    /// If player is hit apply a velocity and move direction 
    /// </summary>
    /// <param name="velocityF"></param>
    /// <param name="time"></param>
    public void HitPlayer(Vector3 velocityF, float time)
    {
        rigidBody.velocity = velocityF;

        pushForce = velocityF.magnitude;
        pushDir = Vector3.Normalize(velocityF);
        StartCoroutine(Decrease(velocityF.magnitude, time));
    }

    private IEnumerator Decrease(float value, float duration)
    {
        //if player is stunned then he can't move
        if (isStuned)
            wasStuned = true;
        isStuned = true;
        canMove = false;

        float delta = 0;
        delta = value / duration;

        for (float t = 0; t < duration; t += Time.deltaTime)
        {
            yield return null;

            //Reduce the force if the ground isnt slide
            if (!slide) 
            {
                pushForce = pushForce - Time.deltaTime * delta;
                pushForce = pushForce < 0 ? 0 : pushForce;
                //Debug.Log(pushForce);
            }
            //Add gravity
            rigidBody.AddForce(new Vector3(0, -gravity * GetComponent<Rigidbody>().mass, 0)); 
        }

        if (wasStuned)
        {
            wasStuned = false;
        }
        else
        {
            isStuned = false;
            canMove = true;
        }
    }

    bool IsGrounded()
    {
        return Physics.Raycast(transform.position, -Vector3.up, distToGround + 0.1f);
    }



    /*private void OnCollisionEnter(Collision collision)
    {
        //Check if player is on ground
        if (collision.gameObject.CompareTag("Ground"))
        {
            isJumping = false;
            
        }

        //check if the ground is slide
        if (collision.gameObject.CompareTag("Slide"))
        {
            isJumping = false;
            slide = true;
        }
        else
        {
            slide = false;
        }

    }*/


}



