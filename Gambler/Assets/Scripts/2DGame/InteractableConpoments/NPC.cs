using System;
using UnityEngine;

public class NPC : MonoBehaviour, IInteractable
{
    public string PromptContent { get; } = "[F] 대화하기";

    private bool isTalking = false;

    private void Start()
    {
        UIManager.Instance.CloseDialog += EndTalking;
    }
    public void OnInteractionEntered()
    {

    }
    public void OnInteractionExited()
    {

    }
    public void OnInteract(KeyCode keyCode)
    {
        if (!isTalking)
        {
            isTalking = true;
            UIManager.Instance.StartDialogue();
        }
    }

    private void EndTalking()
    {
        isTalking = false;
    }
    public bool CanInteract { get; } = true;
    public KeyCode[] KeyCodes { get; } = { KeyCode.F };
}
