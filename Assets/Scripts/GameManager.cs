using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    
    private bool mapOut = false;
    
    void Awake()
    {
        if (!Instance)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
            Destroy(gameObject);
    }
    
    private void OnEnable()
    {
        InputManager.Instance.OnQuit += OnQuit;
    }

    private void OnDisable()
    {
        InputManager.Instance.OnQuit -= OnQuit;
    }
    
    public bool IsMapOut() { return mapOut; }
    public void SetMapState(bool onoff) { mapOut = onoff; }

    private void OnQuit(InputAction.CallbackContext context)
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
