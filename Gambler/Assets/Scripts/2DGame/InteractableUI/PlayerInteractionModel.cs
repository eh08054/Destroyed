using UnityEngine;
using System;

public class PlayerInteractionModel : MonoBehaviour
{
    [SerializeField] private PlayerInteractor playerInteractor;

    public bool IsInteractable { get; private set; }
    public string PromptContent { get; private set; }

    public event Action<bool> OnInteractableChanged;
    
    private void Start()
    {
        playerInteractor.OnHoverEntered += OnInteractableEntered;
        playerInteractor.OnHoverExited += OnInteractableExited;
        playerInteractor.OnInteracted += OnInteracted;
    }

    private void OnInteractableEntered(IInteractable interactable)
    {
        IsInteractable = true;
        PromptContent = interactable.PromptContent;
        OnInteractableChanged?.Invoke(IsInteractable);
    }

    private void OnInteractableExited(IInteractable interactable)
    {
        IsInteractable = false;
        OnInteractableChanged?.Invoke(IsInteractable);
    }
    private void OnInteracted(IInteractable interactable)
    {
        PromptContent = interactable.PromptContent;
        OnInteractableChanged?.Invoke(IsInteractable);
    }
    private void OnDestroy()
    {
        playerInteractor.OnHoverEntered -= OnInteractableEntered;
        playerInteractor.OnHoverExited -= OnInteractableExited;
    }
}
