using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInteraction : MonoBehaviour
{
    [SerializeField] private Transform _head;
    
    [SerializeField] private float _interactionDistance;
    [SerializeField] private LayerMask _layers;
    
    private void OnEnable()
    {
        InputManager.Instance.OnInteract += HandleInteract;
    }

    private void OnDisable()
    {
        InputManager.Instance.OnInteract -= HandleInteract;
    }

    private void HandleInteract(InputAction.CallbackContext ctx)
    {
        Debug.Log("Interact");
        
        // Hacemos un raycast desde la cámara del jugador
        Ray ray = new Ray(_head.position, _head.forward);
        if (!Physics.Raycast(ray, out RaycastHit hit, _interactionDistance, _layers)) return;
        
        // Vemos si el objeto golpeado implementa IInteractable
        IInteractable interactable = hit.collider.GetComponent<IInteractable>();
        interactable?.Interact();
    }
}
