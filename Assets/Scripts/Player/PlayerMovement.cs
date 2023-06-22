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

    [SerializeField] private List<Vector3> spawnPositionList;
    [SerializeField] private float moveSpeed = 7f;
    
    [SerializeField] private float gravityValue = -9.81f;
    [SerializeField] private float slowDownSpeed= 15f;

    private Vector3 verticalMovement;
    private float currentVerticalSpeed;
    private bool isJumping;
    private bool isWalking;

    //height of the jump
    [SerializeField] private float jumpHeight = 1f;

    [SerializeField] private LayerMask collisionsLayerMask;
    [SerializeField] private CinemachineFreeLook cmCamera;

    

    public override void OnNetworkSpawn()
    {
        if (IsOwner)
        {
            _playerMovementInstance = this;
            cmCamera.Priority = 100;
            //playerRigidBody = GetComponent<Rigidbody>();
        }
    }

    private void Update()
    {
        HandleMovement();
    }

    //Check if player is walking
    public bool IsWalking()
    {
        return isWalking;
    }

    /// <summary>
    /// Move player 
    /// </summary>
    private void HandleMovement()
    {
        
        Vector2 inputMovement = PlayerController.Instance.GetPlayerMovement();
        bool jump = PlayerController.Instance.PlayerJumped();
        bool slide = PlayerController.Instance.PlayerSlide();

        Vector3 moveDir = new Vector3(inputMovement.x, 0, inputMovement.y);

        float moveDistance = moveSpeed * Time.deltaTime;
        float playerRadius = .6f;

        //if there is an obstacle returns false
        bool canMove = !Physics.BoxCast(transform.position, Vector3.one * playerRadius, moveDir, Quaternion.identity, moveDistance, collisionsLayerMask);

        if (!canMove)
        {
            /* Cannot move towards moveDir
             Attempt only X movement*/
            Vector3 moveDirX = new Vector3(moveDir.x, 0, 0).normalized;
            canMove = (moveDir.x < -.5f || moveDir.x > +.5f) && !Physics.BoxCast(transform.position, Vector3.one * playerRadius, moveDirX, Quaternion.identity, moveDistance, collisionsLayerMask);

            if (canMove)
            {
                // Can move only on the X
                moveDir = moveDirX;
            }
            else
            {
                /* Cannot move only on the X
                 Attempt only Z movement*/
                Vector3 moveDirZ = new Vector3(0, 0, moveDir.z).normalized;
                canMove = (moveDir.z < -.5f || moveDir.z > +.5f) && !Physics.BoxCast(transform.position, Vector3.one * playerRadius, moveDirZ, Quaternion.identity, moveDistance, collisionsLayerMask);

                if (canMove)
                {
                    // Can move only on the Z
                    moveDir = moveDirZ;
                }
                
            }
        }


        if (canMove)
        {
            transform.position += moveDir * moveDistance;
        }


        isWalking = moveDir != Vector3.zero;

        float rotateSpeed = 10f;
        transform.forward = Vector3.Slerp(transform.forward, moveDir, Time.deltaTime * rotateSpeed);

        //check if player can jump
        if (!isJumping && jump)
        {
            currentVerticalSpeed = Mathf.Sqrt(jumpHeight * -2f * gravityValue);
            isJumping = true;
        }

        // Apply gravity
        currentVerticalSpeed += gravityValue * Time.deltaTime;

        // Apply vertical movement
        verticalMovement = new Vector3(0f, currentVerticalSpeed * Time.deltaTime, 0f);
        transform.position += verticalMovement;

        if (!isJumping)
        {
            currentVerticalSpeed = 0f;
        }
        
        if(slide)
        {
            Vector3 slowDown = new Vector3(0, 0, 0);
            transform.forward = Vector3.Slerp(slowDown, transform.forward, Time.deltaTime * 15f);
        }

    }

    //Returns the NetworkObject component
    public NetworkObject GetNetworkObject()
    {
        return NetworkObject;
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



