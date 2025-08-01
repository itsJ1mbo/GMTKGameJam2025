using UnityEngine;
using UnityEngine.Events;

public class RatationButton : MonoBehaviour, IInteractable
{
    [SerializeField]
    private UnityEvent _onInteract;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    public void Interact()
    {
        Debug.Log("Button");
        _onInteract.Invoke();
    }
}
