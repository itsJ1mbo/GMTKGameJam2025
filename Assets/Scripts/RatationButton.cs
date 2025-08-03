using UnityEngine;
using UnityEngine.Events;

public class RatationButton : MonoBehaviour, IInteractable, ILookAtHandler
{
    [SerializeField]
    private UnityEvent _onInteract;

    [SerializeField]
    private Color interactableColor;
    private Color matColor;
    void Start()
    {
        matColor = GetComponentInChildren<MeshRenderer>().material.GetColor("_Color");
    }
    public void Interact()
    {
        _onInteract.Invoke();
    }

    public void OnLookEnter()
    {
        GetComponentInChildren<MeshRenderer>().material.SetColor("_Color", interactableColor);
        GetComponentInChildren<SkinnedMeshRenderer>().material.SetColor("_Color", interactableColor);
    }

    public void OnLookExit()
    {
        GetComponentInChildren<MeshRenderer>().material.SetColor("_Color", matColor);
        GetComponentInChildren<SkinnedMeshRenderer>().material.SetColor("_Color", matColor);
    }
}
