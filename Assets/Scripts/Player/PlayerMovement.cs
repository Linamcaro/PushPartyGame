using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using Cinemachine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
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

    [Header("Player spawn positions")]
    [SerializeField] private List<Vector3> spawnPositions;

    //events
    public event EventHandler OnPlayerHit;
    public event EventHandler OnPickUpPowerUp;
    public event EventHandler OnPlayerJump;
    public event EventHandler OnPlayerAttack1;
    public event EventHandler OnPlayerStunned;
    public event EventHandler OnPlayerRunning;
    public event EventHandler OnCallSpeed;

    [Header("Movement variables")]
    [SerializeField] private float moveSpeed = 10.0f;
    [SerializeField] private float airVelocity = 8f;
    [SerializeField] private float gravity = 10.0f;
    [SerializeField] private float maxVelocityChange = 10.0f;
    [SerializeField] private float jumpHeight = 2f;
    [SerializeField] private float maxFallSpeed = 20.0f;
    [SerializeField] private float rotateSpeed = 25f;
    [SerializeField] private float SpeedFieldOfView = 75f;
    [SerializeField] private float normalFieldOfView = 60f;
    public Vector3 moveDir { get; private set; }
    private GameObject cam;
    [HideInInspector]public Rigidbody rigidBody;

    [SerializeField] private CinemachineFreeLook cmCamera;

    private float distToGround;

    //Helper Variables
    public bool canMove { get; private set; }
    public bool isRunning { get; private set; }
    public bool isJumping { get; private set; }
    public bool isSliding { get; private set; }

    private bool isStuned = false;
    private bool wasStuned = false;
    private float pushForce;
    private Vector3 pushDir;


    private float speedDelayTime = 10f;
    private float speedDelayTime1 = 1f;

    [Header("Attack variables")]
    public float attackForce = 25f;
    public float attackStunTime = 1f;
    public float attackCoolDown = 0.5f;
    public bool canAttack { get; private set; }
    public bool isAttacking { get; private set; }

    [Header("Particles")]

    public GameObject hitParticles;

    public GameObject liveparticles;


    //-----------------------------------------------------------------------------------------------------------

    private void Start()
    {
        if (!IsOwner) return;

        rigidBody = GetComponent<Rigidbody>();
        rigidBody.freezeRotation = true;
        rigidBody.useGravity = false;

        isSliding = false;
        canMove = true;
        canAttack = true;
        isAttacking = false;

    }

    //-----------------------------------------------------------------------------------------------------------

    public override void OnNetworkSpawn()
    {

        if (IsOwner)
        {
            _playerMovementInstance = this;
            cmCamera.Priority = 100;

            distToGround = GetComponent<Collider>().bounds.extents.y; // get the distance to ground

            cam = Camera.main.gameObject;
        }
        transform.position = spawnPositions[(int)OwnerClientId];
    }

    //-----------------------------------------------------------------------------------------------------------

    private void Update()
    {
        
        if (!IsOwner) return;

        Vector2 inputMovement = PlayerController.Instance.GetPlayerMovement();
        Vector3 verticalMovement = inputMovement.y * cam.transform.forward; //Vertical axis to which I want to move with respect to the camera
        Vector3 horizontalMovement = inputMovement.x * cam.transform.right; //Horizontal axis to which I want to move with respect to the camera
        moveDir = (verticalMovement + horizontalMovement).normalized; //Global position to which I want to move in magnitude 1

        isRunning = moveDir.magnitude > 0;
        if(isRunning)
        {
            OnPlayerRunning?.Invoke(this, EventArgs.Empty);
        }    

        RaycastHit hit;
        if (Physics.Raycast(transform.position, -Vector3.up, out hit, distToGround + 0.1f))
        {
            if (hit.transform.tag == "Slide")
            {
                isSliding = true;
            }
            else
            {
                isSliding = false;
            }
        }

    }

    //-----------------------------------------------------------------------------------------------------------

    private void FixedUpdate()
    {
        if (!IsOwner)
        {
            return;
        }

        HandleMovement();
    }

    //-----------------------------------------------------------------------------------------------------------
    /// <summary>
    /// Move the player according to the user input 
    /// </summary>
    private void HandleMovement()
    {
        if (!PushPartyGameManager.Instance.IsGamePlaying())
        {
            return;
        }

        bool jump = PlayerController.Instance.PlayerJumped();
        bool Attack1 = PlayerController.Instance.PlayerFired1();

        if (canMove)
        {
            if (moveDir.x != 0 || moveDir.z != 0)
            {

                Vector3 targetDir = moveDir;//Direction of the character

                targetDir.y = 0;
                if (targetDir == Vector3.zero)
                    targetDir = transform.forward;

                Quaternion tr = Quaternion.LookRotation(targetDir);//Rotation of the character to where it moves
                Quaternion targetRotation = Quaternion.Slerp(transform.rotation, tr, Time.deltaTime * rotateSpeed);//Rotate the character smoothly(little by little)
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

                if (!isSliding)
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
                    OnPlayerJump?.Invoke(this, EventArgs.Empty);
                    isJumping = true;
                    rigidBody.velocity = new Vector3(velocity.x, CalculateJumpVerticalSpeed(), velocity.z);
                    
                }

                if (Attack1 && canAttack)
                {
                   
                    OnPlayerAttack1?.Invoke(this, EventArgs.Empty);
                    canAttack = false;
                    Invoke(nameof(ResetAttack), attackCoolDown);
                }

            }
            else
            {
                if (!isSliding)
                {
                    //air movement
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

    //-----------------------------------------------------------------------------------------------------------
    /// <summary>
    /// From the jump height and gravity we deduce the upwards speed 
    /// for the character to reach at the apex.
    /// </summary>
    /// <returns></returns>
    float CalculateJumpVerticalSpeed()
    {
        return Mathf.Sqrt(2 * jumpHeight * gravity);
    }

    //-----------------------------------------------------------------------------------------------------------
    /// <summary>
    /// If player is hit apply a velocity and move direction 
    /// </summary>
    /// <param name="velocityF"></param>
    /// <param name="time"></param>
    public void HitPlayer(Vector3 velocityF, float time)
    {
        OnPlayerHit?.Invoke(this, EventArgs.Empty);
        rigidBody = GetComponent<Rigidbody>();

        if (rigidBody == null) Debug.LogError("RB error");
        if (velocityF == null) Debug.LogError("velF error");

        Instantiate(hitParticles, transform.position + new Vector3(0f, 1.5f, 0f), Quaternion.identity);

        rigidBody.velocity = velocityF;

        pushForce = velocityF.magnitude;
        pushDir = Vector3.Normalize(velocityF);
        StartCoroutine(Decrease(velocityF.magnitude, time));
    }

    //-----------------------------------------------------------------------------------------------------------
    public void LivePlayer()
    {
        Instantiate(liveparticles, transform.position + new Vector3(175.1f, 387.5f, 0f), Quaternion.identity);
    }
    private IEnumerator Decrease(float value, float duration)
    {
        isStuned = true;
        //if player is stunned then he can't move
        if (isStuned)
        {
            OnPlayerStunned?.Invoke(this, EventArgs.Empty);
            wasStuned = true;
            canMove = false;

        }

        float delta = 0;
        delta = value / duration;

        for (float t = 0; t < duration; t += Time.deltaTime)
        {
            yield return null;

            //Reduce the force if the ground isnt slide
            if (!isSliding)
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
            isStuned = false;
            canMove = true;
        }
        
    }

    //-----------------------------------------------------------------------------------------------------------

    public bool IsGrounded()
    {
        return Physics.Raycast(transform.position, -Vector3.up, distToGround + 0.1f);
    }

    //-----------------------------------------------------------------------------------------------------------

    public void UpdateSpeed(float speed)
    {
        moveSpeed = speed;
    }

    //-----------------------------------------------------------------------------------------------------------

    public IEnumerator SpeedEnum(Collider player)
    {
        OnCallSpeed?.Invoke(this, EventArgs.Empty);
        UpdateSpeed(20f);
        //cmCamera.m_Lens.FieldOfView = SpeedFieldOfView;
        yield return new WaitForSeconds(speedDelayTime);

    }

    //-----------------------------------------------------------------------------------------------------------

    public IEnumerator SpeedEnum1(Collider player)
    {
        UpdateSpeed(7f);
        //cmCamera.m_Lens.FieldOfView = normalFieldOfView;
        yield return new WaitForSeconds(speedDelayTime1);
        

    }

    //-----------------------------------------------------------------------------------------------------------

    public void CallSpeed(Collider player)
    {
        OnPickUpPowerUp?.Invoke(this, EventArgs.Empty);
        StartCoroutine(ChangeSpeed(player));
    }

    //-----------------------------------------------------------------------------------------------------------

    private IEnumerator ChangeSpeed(Collider player)
    {
        yield return StartCoroutine(SpeedEnum(player));
        
        yield return StartCoroutine(SpeedEnum1(player));

    }


    //-----------------------------------------------------------------------------------------------------------

    public float getVelocity()
    {
        return rigidBody.velocity.magnitude;
    }

    //-----------------------------------------------------------------------------------------------------------
    void ResetAttack()
    {
        canAttack = true;
    }

    //-----------------------------------------------------------------------------------------------------------
    public void ActivatePunch()
    {
        isAttacking = true;
    }

    //-----------------------------------------------------------------------------------------------------------
    public void DeactivatePunch()
    {
        isAttacking = false;
    }

}



