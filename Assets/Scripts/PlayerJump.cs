using System;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
public class PlayerJump : MonoBehaviour
{
    private CharacterController _controller;
    private Vector3 _velocity;
    private bool _jumpInput;
    private bool _isGrounded;

    [SerializeField] private float _jumpHeight = 2f;
    [SerializeField] private float _gravity = -9.81f;
    [SerializeField] private float _gravityScale = 1f;
    
    [SerializeField] private Transform _feet;
    [SerializeField] private LayerMask _groundMask;

    private void OnEnable()
    {
        InputManager.Instance.OnJump += OnJump;
    }

    private void OnDisable()
    {
        InputManager.Instance.OnJump -= OnJump;
    }

    private void Start()
    {
        _controller = GetComponent<CharacterController>();
    }

    private void OnJump(InputAction.CallbackContext context)
    {
        _jumpInput = true;
    }

    private void Update()
    {
        if (_isGrounded && _velocity.y < 0f)
        {
            _velocity.y = -2f;
        }
        
        if (_jumpInput && _isGrounded)
        {
            _velocity.y = Mathf.Sqrt(_jumpHeight * -2f * _gravity * _gravityScale);
        }

        _jumpInput = false;

        _velocity.y += _gravity * _gravityScale * Time.deltaTime;

        _controller.Move(_velocity * Time.deltaTime);
        
        _isGrounded = Physics.CheckBox(_feet.position, new Vector3(0.25f, 0.1f, 0.25f), _feet.rotation, _groundMask);
    }
}