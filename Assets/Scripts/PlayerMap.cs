using System;
using UnityEngine;
using DG.Tweening;
using UnityEngine.InputSystem;

public class PlayerMap : MonoBehaviour
{
    [SerializeField]
    private Transform _mapPos;

    private void OnUseMap(InputAction.CallbackContext context)
    {
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
