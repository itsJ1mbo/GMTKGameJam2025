using UnityEngine;
using UnityEngine.Events;

public class RatationButton : MonoBehaviour, IInteractable
{
    [SerializeField]
    private UnityEvent _onInteract;
    
    public void Interact()
    {
        _onInteract.Invoke();
    }
}
