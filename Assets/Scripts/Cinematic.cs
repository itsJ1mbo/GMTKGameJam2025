using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;

public class Cinematic : MonoBehaviour
{
    [Header("Referencias UI")]
    [SerializeField] private GameObject _button;
    [SerializeField] private Image _whiteScreen;

    [Header("Duraciones (segundos)")]
    [SerializeField] private float _initialDelay = 3f;
    [SerializeField] private float _fadeDuration = 1.2f;
    [SerializeField] private float _holdWhite = 0.5f;

    public void StartGame()
    {
        StartCoroutine(StartCinematic());
    }

    private IEnumerator StartCinematic()
    {
        _button.SetActive(false);

        // Retraso antes del fade (puede ajustar por animaciones u otros eventos)
        yield return new WaitForSeconds(_initialDelay);

        // Paso 1: Fade a blanco
        yield return FadeAlpha(0f, 1f, _fadeDuration);

        // Mantiene pantalla blanca
        yield return new WaitForSeconds(_holdWhite);

        // Aquí puedes cargar la siguiente escena o desactivar el overlay si vas a volver al juego
        // yield return FadeAlpha(from: 1f, to: 0f, duration: _fadeDuration);
        // _whiteScreen.gameObject.SetActive(false);

        // EJEMPLO Post‑fade:
        SceneManager.LoadScene("Game");
    }

    private IEnumerator FadeAlpha(float from, float to, float duration)
    {
        float elapsed = 0f;
        Color col = _whiteScreen.color;
        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / duration);
            float alpha = Mathf.Lerp(from, to, t);
            _whiteScreen.color = new Color(col.r, col.g, col.b, alpha);
            yield return null;
        }
        _whiteScreen.color = new Color(col.r, col.g, col.b, to);
    }
}
