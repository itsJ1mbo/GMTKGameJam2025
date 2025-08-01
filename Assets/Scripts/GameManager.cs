using UnityEngine;

public class GameManager : MonoBehaviour
{ 
    public static GameManager Instance { get; private set; }
    
    private bool cubeRotating = false;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }
    
    public bool isCubeRotating() { return cubeRotating; }
    public void setCubeRotation(bool onoff) { cubeRotating = onoff; }
}
