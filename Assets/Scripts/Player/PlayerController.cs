using UnityEngine;
using Unity.Netcode;
using Cinemachine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    private static PlayerController _instance;

    public static PlayerController Instance
    {
        get
        {
            return _instance;
        }
    }


    private InputActions playerInputAction;

    private void Awake()
    {
        _instance = this;
        
        playerInputAction = new InputActions();

    }

    private void OnEnable()
    {
        playerInputAction.Enable();

    }

    private void OnDisable()
    {
        playerInputAction.Disable();
    }

    private void OnDestroy()
    {
        playerInputAction.Dispose();

    }

    //helper function to call the player movement controls
    public Vector2 GetPlayerMovement()
    {
        Vector2 inputMovement = playerInputAction.Player.Move.ReadValue<Vector2>();
        inputMovement = inputMovement.normalized;

        return inputMovement;
    }

    //Returns true if the jump control was triggered
    public bool PlayerJumped()
    {
        return playerInputAction.Player.Jump.triggered;
    }


    //Returns true if the fire control was triggered
    public bool PlayerFired1()
    {
        return playerInputAction.Player.Fire1.triggered;
    }
    //Returns true if the fire control was triggered
    public bool PlayerFired2()
    {
        return playerInputAction.Player.Fire1.triggered;
    }

    //Returns true if the jump control was triggered
    public bool PlayerSlide()
    {
        return playerInputAction.Player.Slide.triggered;
    }
}