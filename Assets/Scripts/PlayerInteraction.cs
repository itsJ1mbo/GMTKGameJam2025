using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInteraction : MonoBehaviour
{
    [SerializeField] private Transform _head;
    
    [SerializeField] private float _interactionDistance;
    [SerializeField] private LayerMask _layers;
    private ILookAtHandler currentLookTarget;
    private void OnEnable()
    {
        InputManager.Instance.OnInteract += HandleInteract;
    }

    private void OnDisable()
    {
        InputManager.Instance.OnInteract -= HandleInteract;
    }

    void Update()
    {
        Ray ray = new Ray(_head.transform.position, _head.transform.forward);
        if (Physics.Raycast(ray, out RaycastHit hit, _interactionDistance))
        {
            ILookAtHandler lookTarget = hit.collider.GetComponent<ILookAtHandler>();

            if (lookTarget != null)
            {
                if (lookTarget != currentLookTarget)
                {
                    currentLookTarget?.OnLookExit();
                    currentLookTarget = lookTarget;
                    currentLookTarget.OnLookEnter();
                }
            }
            else
            {
                currentLookTarget?.OnLookExit();
                currentLookTarget = null;
            }
        }
        else
        {
            currentLookTarget?.OnLookExit();
            currentLookTarget = null;
        }
    }
    
    private void HandleInteract(InputAction.CallbackContext ctx)
    {
        // Hacemos un raycast desde la cámara del jugador
        Ray ray = new Ray(_head.position, _head.forward);
        if (!Physics.Raycast(ray, out RaycastHit hit, _interactionDistance, _layers)) return;
        
        // Vemos si el objeto golpeado implementa IInteractable
        IInteractable interactable = hit.collider.GetComponent<IInteractable>();
        interactable?.Interact();
    }
}
