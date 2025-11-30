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
    
    [SerializeField] private float rectAnimDuration = 0.5f; // velocidad del efecto
    private bool isAnimatingRect = false;

    private Camera _camera;

    private void Start()
    {
        _camera = Camera.main;
    }

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
        if (isRestarting || isAnimatingRect) return;

        // Iniciar animación del rect
        StartCoroutine(AnimateViewportRect(
            new Rect(0.15f, 0.25f, 0.7f, 0.7f)
        ));

        StartCoroutine(Restart());
    }

    private IEnumerator AnimateViewportRect(Rect targetRect)
    {
        isAnimatingRect = true;

        _camera.ResetAspect();
        _camera.clearFlags = CameraClearFlags.SolidColor;
        _camera.backgroundColor = Color.black;

        Rect initial = _camera.rect;
        float t = 0f;

        while (t < 1f)
        {
            t += Time.deltaTime / rectAnimDuration;

            // easing suave (SmootherStep)
            float smooth = t * t * (3f - 2f * t);

            _camera.rect = LerpRect(initial, targetRect, smooth);

            yield return null;
        }

        _camera.rect = targetRect;

        isAnimatingRect = false;
    }

    private Rect LerpRect(Rect a, Rect b, float t)
    {
        return new Rect(
            Mathf.Lerp(a.x, b.x, t),
            Mathf.Lerp(a.y, b.y, t),
            Mathf.Lerp(a.width, b.width, t),
            Mathf.Lerp(a.height, b.height, t)
        );
    }

    private IEnumerator Restart()
    {
        isRestarting = true;

        if (!restartSound.IsNull)
            RuntimeManager.PlayOneShot(restartSound);

        yield return new WaitForSeconds(delay);

        // Restaurar rect full screen antes de cargar escena

        yield return StartCoroutine(AnimateViewportRect(new Rect(0f, 0f, 1f, 1f)));
        
        SceneManager.LoadScene("Game");
    }

}
