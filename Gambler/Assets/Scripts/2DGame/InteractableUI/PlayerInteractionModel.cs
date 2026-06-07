using UnityEngine;
using System;

public class PlayerInteractionModel : MonoBehaviour
{
    [SerializeField] private PlayerInteractor playerInteractor;

    public bool IsInteractable { get; private set; }
    public string PromptContent { get; private set; }

    public event Action<bool, GameObject> OnInteractableChanged;
    
    private void Start()
    {
        playerInteractor.OnHoverEntered += OnInteractableEntered;
        playerInteractor.OnHoverExited += OnInteractableExited;
        playerInteractor.OnInteracted += OnInteracted;
    }

    private void OnInteractableEntered(IInteractable interactable, GameObject interactableObject)
    {
        IsInteractable = true;
        PromptContent = interactable.PromptContent;
        OnInteractableChanged?.Invoke(IsInteractable, interactableObject);
    }

    private void OnInteractableExited(IInteractable interactable, GameObject interactableObject)
    {
        IsInteractable = false;
        OnInteractableChanged?.Invoke(IsInteractable, interactableObject);
    }
    private void OnInteracted(IInteractable interactable, GameObject interactableObject)
    {
        PromptContent = interactable.PromptContent;
        OnInteractableChanged?.Invoke(IsInteractable, interactableObject);
    }
    private void OnDestroy()
    {
        playerInteractor.OnHoverEntered -= OnInteractableEntered;
        playerInteractor.OnHoverExited -= OnInteractableExited;
    }
}
