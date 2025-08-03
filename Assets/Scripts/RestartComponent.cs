using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class RestartComponent : MonoBehaviour
{
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
        SceneManager.LoadScene("Game");
    }
}
