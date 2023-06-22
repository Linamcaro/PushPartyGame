using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using Cinemachine;

[RequireComponent(typeof(Rigidbody))]
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

    [SerializeField] private LayerMask collisionsLayerMask;

    private bool isWalking;

    public override void OnNetworkSpawn()
    {
        if (IsOwner)
        {
            _playerMovementInstance = this;
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

        Vector3 moveDir = new Vector3(inputMovement.x, 0f, inputMovement.y);

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
                else
                {
                    // Cannot move in any direction
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
    }

    //Returns the NetworkObject component
    public NetworkObject GetNetworkObject()
    {
        return NetworkObject;
    }

}



