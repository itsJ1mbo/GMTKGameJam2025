using System;
using UnityEngine;
using UnityEngine.InputSystem;
using FMODUnity;

[RequireComponent(typeof(CharacterController))]
public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private Transform _headTr;
    
    private InputManager _input;
    private CharacterController _controller;
    private Vector2 _movement;
    
    [SerializeField] private float _speed;
    
    private Vector3 _dir;
    private Vector3 _velocity;

    [SerializeField] private EventReference footstepSound; 
    [SerializeField] private float stepInterval = 0.5f;    
    private float stepTimer;

    private void OnMove(InputAction.CallbackContext context)
    {
        _movement = context.ReadValue<Vector2>();
    }

    private void OnStop(InputAction.CallbackContext context)
    {
        _movement = Vector2.zero;
    }

    private void Awake()
    {
        _input = InputManager.Instance;
    }

    private void OnEnable()
    {
        _input.OnMovement += OnMove;
        _input.OnMoveStop += OnStop;
    }

    private void OnDisable()
    {
        _input.OnMovement -= OnMove;
        _input.OnMoveStop -= OnStop;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _controller = GetComponent<CharacterController>();   
    }
    
    // Update is called once per frame
    void Update()
    {
        Vector2 input = _movement;
        
        _dir = new Vector3(input.x, 0, input.y);
        _dir = _headTr.forward * _dir.z + _headTr.right * _dir.x;
        _dir.y = 0;
        _dir.Normalize();
        
        _controller.Move(_dir * (Time.deltaTime * _speed));

        HandleFootsteps(input);
    }

    private void HandleFootsteps(Vector2 input)
    {

        Debug.Log($"Input: {input.sqrMagnitude}, Grounded: {_controller.isGrounded}");
        if (input.sqrMagnitude > 0.01f && _controller.isGrounded)
        {
            stepTimer -= Time.deltaTime; 

            if (stepTimer <= 0f)
            {
                RuntimeManager.PlayOneShot(footstepSound, transform.position);
                stepTimer = stepInterval; 
            }
        }
        else
        {
            stepTimer = 0f;
        }
    }
}
