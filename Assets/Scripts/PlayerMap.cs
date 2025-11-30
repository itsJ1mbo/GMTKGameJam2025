using System;
using UnityEngine;
using DG.Tweening;
using UnityEngine.InputSystem;
using FMODUnity;

public class PlayerMap : MonoBehaviour
{
    [SerializeField]
    private Transform _mapPos;

    [SerializeField] private EventReference _mapOpenSound;
    private bool _isMapOpen = false;

    public bool IsMapOpen
    {
        get { return _isMapOpen; }
    }

    private void OnUseMap(InputAction.CallbackContext context)
    {

        if (!_isMapOpen)
        {
            RuntimeManager.PlayOneShot(_mapOpenSound);
            _isMapOpen = true;
        }

        transform.DOMove(_mapPos.position, 1);
        GameManager.Instance.SetMapState(true);
    }

    private void OnEnable()
    {
        InputManager.Instance.OnMap += OnUseMap;
    }

    private void OnDisable()
    {
        InputManager.Instance.OnMap -= OnUseMap;
    }
}
