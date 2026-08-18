using System;
using UnityEngine;
using TMPro;

public class NPC : MonoBehaviour, IInteractable
{
    public string PromptContent { get; private set; } = "[F] 대화하기";
    [SerializeField] private TMP_Text text;

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
        text.gameObject.SetActive(false);
        if (!isTalking)
        {
            isTalking = true;
            UIManager.Instance.StartDialogue();
        }
    }

    private void EndTalking()
    {
        text.gameObject.SetActive(true);
        isTalking = false;
    }
    public bool CanInteract { get; } = true;
    public KeyCode[] KeyCodes { get; } = { KeyCode.F };
}
