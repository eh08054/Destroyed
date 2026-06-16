using System;
using UnityEngine;
public interface IInteractable
{
    string PromptContent { get; }
    void OnInteractionEntered();
    void OnInteractionExited();
    void OnInteract(KeyCode keyCode);
    bool CanInteract { get; }
    KeyCode[] KeyCodes { get; }
}
