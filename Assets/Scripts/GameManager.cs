using UnityEngine;

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
    
    public bool IsMapOut() { return mapOut; }
    public void SetMapState(bool onoff) { mapOut = onoff; }
}
