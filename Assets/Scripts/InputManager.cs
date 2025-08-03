using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    public static InputManager Instance { get; private set; }

    private InputActions _inputActions;
    public event Action<InputAction.CallbackContext> OnMovement;
    public event Action<InputAction.CallbackContext> OnMoveStop;
    public event Action<InputAction.CallbackContext> OnLook;
    public event Action<InputAction.CallbackContext> OnLookStop;
    public event Action<InputAction.CallbackContext> OnInteract;
    public event Action<InputAction.CallbackContext> OnMap;
    public event Action<InputAction.CallbackContext> OnJump;
    public event Action<InputAction.CallbackContext> OnRestart;

    private void OnMovementPerformed(InputAction.CallbackContext ctx) => OnMovement?.Invoke(ctx);
    private void OnMovementCanceled(InputAction.CallbackContext ctx) => OnMoveStop?.Invoke(ctx);
    private void OnLookPerformed(InputAction.CallbackContext ctx) => OnLook?.Invoke(ctx);
    private void OnLookCanceled(InputAction.CallbackContext ctx) => OnLookStop?.Invoke(ctx);
    private void OnInteractPerformed(InputAction.CallbackContext ctx) => OnInteract?.Invoke(ctx);
    private void OnMapUsed(InputAction.CallbackContext ctx) => OnMap?.Invoke(ctx);
    private void OnJumpPreformed(InputAction.CallbackContext ctx) => OnJump?.Invoke(ctx);
    private void OnPlayerRestart(InputAction.CallbackContext ctx) => OnRestart?.Invoke(ctx);
    
    public void EnablePlayerInput()
    {
        _inputActions.UI.Disable();
        _inputActions.Player.Enable();
    }
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        if (!Instance)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
                    
            _inputActions = new InputActions();    
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    private void OnEnable()
    {
        if (_inputActions == null) return;
        
        _inputActions.Enable();
        
        _inputActions.Player.Move.performed += OnMovementPerformed;
        _inputActions.Player.Move.canceled += OnMovementCanceled;
        
        _inputActions.Player.Look.performed += OnLookPerformed;
        _inputActions.Player.Look.canceled += OnLookCanceled;
        
        _inputActions.Player.Interact.performed += OnInteractPerformed;
        
        _inputActions.Player.Map.performed += OnMapUsed;
        
        _inputActions.Player.Jump.performed += OnJumpPreformed;
        
        _inputActions.Player.Restart.performed += OnPlayerRestart;
        
        EnablePlayerInput();
    }

    private void OnDisable()
    {
        if (Instance != this || _inputActions == null) 
            return;
        
        _inputActions.Player.Move.performed -= OnMovementPerformed;
        _inputActions.Player.Move.canceled -= OnMovementCanceled;
        
        _inputActions.Player.Look.performed -= OnLookPerformed;
        _inputActions.Player.Look.canceled -= OnLookCanceled;
        
        _inputActions.Player.Interact.performed -= OnInteractPerformed;
        
        _inputActions.Player.Map.performed -= OnMapUsed;
        
        _inputActions.Player.Jump.performed -= OnJumpPreformed;
        
        _inputActions.Player.Restart.performed -= OnPlayerRestart;
        
        _inputActions.Disable();
        
    }
}
