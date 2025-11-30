using FMODUnity;
using UnityEngine;
using UnityEngine.Events;

public class RatationButton : MonoBehaviour, IInteractable, ILookAtHandler
{
    [SerializeField]
    private UnityEvent _onInteract;

    [SerializeField]
    private Color interactableColor;
    private Color matColor;

    [SerializeField] private EventReference sound;

    [SerializeField] private bool highlight = true;
    void Start()
    {
        matColor = GetComponentInChildren<MeshRenderer>().material.GetColor("_Color");
    }
    public void Interact()
    {
        RuntimeManager.PlayOneShot(sound, transform.position);
        _onInteract.Invoke();
    }

    public void OnLookEnter()
    {
        if (highlight)
        { 
            GetComponentInChildren<MeshRenderer>().material.SetColor("_Color", interactableColor); 
            GetComponentInChildren<SkinnedMeshRenderer>().material.SetColor("_Color", interactableColor);
        }
    }

    public void OnLookExit()
    {
        if (highlight)
        { 
            GetComponentInChildren<MeshRenderer>().material.SetColor("_Color", matColor);
            GetComponentInChildren<SkinnedMeshRenderer>()?.material.SetColor("_Color", matColor);
        }
    }
}
