using System;
using UnityEngine;

public class NPC : MonoBehaviour, IInteractable
{
    public string PromptContent { get; } = "[F] 대화하기";
    public void OnInteractionEntered()
    {

    }
    public void OnInteractionExited()
    {

    }
    public void OnInteract(KeyCode keyCode)
    {
        GameManager.Instance.StartDialogue();
    }
    public bool CanInteract { get; } = true;
    public KeyCode[] KeyCodes { get; } = { KeyCode.F };
}
