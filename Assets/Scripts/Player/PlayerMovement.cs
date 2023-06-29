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

    //Player movement speed
    [SerializeField] private float moveSpeed = 7f;
    [SerializeField] private float slowDownSpeed= 15f;

    //Player jumping
    [SerializeField] private float jumpHeight = 1f;
    [SerializeField] private float jumpForce = 70f;



    [SerializeField] private LayerMask collisionsLayerMask;
    [SerializeField] private CinemachineFreeLook cmCamera;
    private Rigidbody rigidBody;

    //Helper Variables
    private bool isJumping;
    private bool isWalking;
    private bool isSliding;

    public override void OnNetworkSpawn()
    {

        if (IsOwner)
        {
            _playerMovementInstance = this;
            cmCamera.Priority = 100;
            rigidBody = GetComponent<Rigidbody>();
        }

        transform.position = spawnPositions[(int)OwnerClientId];

    }

    private void Update()
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
                //Can move only on the X
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
                    //Can move only on the Z
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
            isJumping = true;
            //currentVerticalSpeed = Mathf.Sqrt(jumpHeight * -2f * gravityValue);
            rigidBody.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            
        }

        if(slide)
        {
            isSliding = true;
        }
        else
        {
            isSliding = false;
        }
        

    }

   //Check if player is on ground
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isJumping = false;
            
        }
    }


}



