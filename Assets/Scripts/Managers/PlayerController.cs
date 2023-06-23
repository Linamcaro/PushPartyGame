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

    /// <summary>
    /// helper function to call the player movement controls
    /// </summary>
    /// <returns></returns>
    public Vector2 GetPlayerMovement()
    {
        Vector2 inputMovement = playerInputAction.Player.Move.ReadValue<Vector2>();
        inputMovement = inputMovement.normalized;

        return inputMovement;
    }

    /// <summary>
    /// Returns true if the jump control was triggered
    /// </summary>
    /// <returns></returns>
    public bool PlayerJumped()
    {
        return playerInputAction.Player.Jump.triggered;
    }


    /// <summary>
    /// /Returns true if the fire control was triggered
    /// </summary>
    /// <returns></returns>
    public bool PlayerFired1()
    {
        return playerInputAction.Player.Fire1.triggered;
    }
    /// <summary>
    /// Returns true if the fire control was triggered
    /// </summary>
    /// <returns></returns>
    public bool PlayerFired2()
    {
        return playerInputAction.Player.Fire1.triggered;
    }

    /// <summary>
    /// Returns true if the jump control was triggered
    /// </summary>
    /// <returns></returns>
    public bool PlayerSlide()
    {
        return playerInputAction.Player.Slide.triggered;
    }
}