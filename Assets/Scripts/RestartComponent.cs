using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class RestartComponent : MonoBehaviour
{
    [SerializeField]
    private GameObject _player;
    [SerializeField]
    private Transform _spawn;
    [SerializeField]
    private LayerMask _playerMask;
    
    private Transform _tr;
    private MeshRenderer _mesh;
    
    private bool _isOutside = false;

    private void OnEnable()
    {
        InputManager.Instance.OnRestart += OnRestart;
    }

    private void OnDisable()
    {
        InputManager.Instance.OnRestart -= OnRestart;
    }

    private void OnRestart(InputAction.CallbackContext context)
    {
        _isOutside = Physics.CheckBox(_tr.position, _mesh.bounds.extents * 2, _tr.rotation, _playerMask);

        if (_isOutside)
        {
            _player.GetComponent<CharacterController>().enabled = false;
            _player.transform.position = _spawn.position;
            _player.GetComponent<CharacterController>().enabled = true;
        }
    }

    private void Start()
    {
        _tr = transform;
        _mesh = GetComponent<MeshRenderer>();
    }
}
