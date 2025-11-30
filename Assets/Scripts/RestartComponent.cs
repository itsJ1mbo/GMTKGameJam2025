using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using FMODUnity;
using System.Collections;

public class RestartComponent : MonoBehaviour
{
    [SerializeField] private EventReference restartSound;
    [SerializeField] private float delay = 8.2f;
    private bool isRestarting = false;


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
        if (isRestarting) return;

        StartCoroutine(Restart());
    }

    private IEnumerator Restart()
    {
        isRestarting = true;

        if (!restartSound.IsNull)
        {
            RuntimeManager.PlayOneShot(restartSound);
        }

        yield return new WaitForSeconds(delay);

        SceneManager.LoadScene("Game");
    }
}
