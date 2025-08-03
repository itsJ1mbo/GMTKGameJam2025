using UnityEngine;
using UnityEngine.SceneManagement;

public class Timer : MonoBehaviour
{
    private float _time = 0;
    private float _maxTime = 1.5f;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _time = 0;
        _maxTime = 1.5f;
    }

    // Update is called once per frame
    void Update()
    {
        _time += Time.deltaTime;
        
        if (_time > _maxTime)
        {
            SceneManager.LoadScene("Game");
        }
    }
}
