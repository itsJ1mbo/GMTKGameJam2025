using System;
using FMOD.Studio;
using UnityEngine;
using UnityEngine.InputSystem;
using FMODUnity;
using STOP_MODE = FMOD.Studio.STOP_MODE;

[RequireComponent(typeof(CharacterController))]
public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private Transform _headTr;
    [SerializeField] private Transform _feetTr;
    
    private InputManager _input;
    private CharacterController _controller;
    private PlayerJump _jump;
    
    [SerializeField] private float _speed;
    
    private Vector2 _movement;
    private Vector3 _dir;
    private Vector3 _velocity;
    private bool _outside;
    private bool _counting;

    private EventInstance _eventInstance;
    [SerializeField] private EventReference footstepSound;
    private bool _walking;

    private float _lastGrounded;

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
        _jump = GetComponent<PlayerJump>();
        _eventInstance = RuntimeManager.CreateInstance(footstepSound);
        _eventInstance.set3DAttributes(RuntimeUtils.To3DAttributes(_feetTr.position));
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
        _outside = Physics.CheckBox(_feetTr.position, new Vector3(0.25f, 0.1f, 0.25f), _feetTr.rotation, LayerMask.NameToLayer("Outside"));

        _eventInstance.set3DAttributes(RuntimeUtils.To3DAttributes(_feetTr.position));
        _eventInstance.setParameterByName("Outside", _outside ? 0 : 1);
        
        if(!_jump.IsGrounded && !_counting)
        {
            _counting = true;
            _lastGrounded = Time.time;
        }
        
        if (input.sqrMagnitude > 0.01f && _jump.IsGrounded && !_walking)
        {
            _eventInstance.start();
            _walking = true;
        }
        else if (!(input.sqrMagnitude > 0.01f) || (!_jump.IsGrounded && Time.time - _lastGrounded > 0.5f))
        {
            _eventInstance.stop(STOP_MODE.ALLOWFADEOUT);
            _walking = false;
            _counting = false;
        }
    }
}
