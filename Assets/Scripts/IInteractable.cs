using UnityEngine;

public interface IInteractable
{
    void Interact();
}
public interface ILookAtHandler
{
    void OnLookEnter();
    void OnLookExit();
}