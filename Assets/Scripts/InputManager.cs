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

    private void OnMovementPerformed(InputAction.CallbackContext ctx) => OnMovement?.Invoke(ctx);
    private void OnMovementCanceled(InputAction.CallbackContext ctx) => OnMoveStop?.Invoke(ctx);
    private void OnLookPerformed(InputAction.CallbackContext ctx) => OnLook?.Invoke(ctx);
    private void OnLookCanceled(InputAction.CallbackContext ctx) => OnLookStop?.Invoke(ctx);
    private void OnInteractPerformed(InputAction.CallbackContext ctx) => OnInteract?.Invoke(ctx);
    
    public void EnablePlayerInput()
    {
        _inputActions.UI.Disable();
        _inputActions.Player.Enable();
    }
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
            Destroy(gameObject);
        
        _inputActions = new InputActions();    
    }

    private void OnEnable()
    {
        _inputActions.Enable();
        
        _inputActions.Player.Move.performed += OnMovementPerformed;
        _inputActions.Player.Move.canceled += OnMovementCanceled;
        
        _inputActions.Player.Look.performed += OnLookPerformed;
        _inputActions.Player.Look.canceled += OnLookCanceled;
        
        _inputActions.Player.Interact.performed += OnInteractPerformed;
        
        EnablePlayerInput();
    }

    private void OnDisable()
    {
        _inputActions.Player.Move.performed -= OnMovementPerformed;
        _inputActions.Player.Move.canceled -= OnMovementCanceled;
        
        _inputActions.Player.Look.performed -= OnLookPerformed;
        _inputActions.Player.Look.canceled -= OnLookCanceled;
        
        _inputActions.Player.Interact.performed -= OnInteractPerformed;
        
        _inputActions.Disable();
        
    }
}
