using System;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private Transform _headTr;
    
    private InputManager _input;
    private CharacterController _controller;
    private Vector2 _movement;
    
    [SerializeField] private float _speed;
    [SerializeField] private float _gravity = -9.81f;

    private Vector3 _dir;
    private Vector3 _velocity;

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
        
        if(!_controller.isGrounded)
        {
            _velocity.y += _gravity * Time.deltaTime;
            _controller.Move(_velocity * Time.deltaTime);
        }
    }
}
