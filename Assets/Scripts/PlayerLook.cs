using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerLook : MonoBehaviour
{
    private InputManager _input;
    private Transform _tr;
    
    [SerializeField] private float _sens = 1.0f;
    [SerializeField] private float _clampAngle = 80.0f;

    private Vector2 _mouseDelta;
    // Giro sobre eje X
    private float _rotationX;
    // Giro sobre eje Y
    private float _rotationY;

    private void OnLook(InputAction.CallbackContext context)
    {
        _mouseDelta = context.ReadValue<Vector2>();
    }

    private void OnStop(InputAction.CallbackContext context)
    { 
        _mouseDelta = Vector2.zero;
    }

    private void Awake()
    {
        _input = InputManager.Instance;
    }

    private void OnEnable()
    {
        _input.OnLook += OnLook;
        _input.OnLookStop += OnStop;
    }

    private void OnDisable()
    {
        _input.OnLook -= OnLook;
        _input.OnLookStop -= OnStop;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        _tr = transform;

        Vector3 localEulerAngles = _tr.localEulerAngles;
        _rotationX = localEulerAngles.x;
        _rotationY = localEulerAngles.y;
    }

    private void Update()
    {
        _rotationY += _mouseDelta.x * _sens * Time.deltaTime;
        _rotationX -= _mouseDelta.y * _sens * Time.deltaTime;
        _rotationX = Mathf.Clamp(_rotationX, -_clampAngle, _clampAngle);
        
        _tr.localRotation = Quaternion.Euler(_rotationX, _rotationY, 0f);
    }
}
