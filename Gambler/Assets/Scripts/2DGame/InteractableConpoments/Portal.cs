using System;
using UnityEngine;

public class Portal : MonoBehaviour, IInteractable
{
    [SerializeField] private Sprite openSprite;
    [SerializeField] private Sprite closedSprite;
    [SerializeField] private int nextStage;
    [SerializeField] private string promptContent;
    [SerializeField] private Animator animator;
    public string PromptContent => promptContent;
    private bool isOpened = false;
    public void OnEnable()
    {
        GameManager.Instance.OnClear += ClearPortal;
    }
    public void OnInteractionEntered()
    {

    }
    public void OnInteractionExited()
    {

    }
    public void OnInteract(KeyCode keyCode)
    {
        if(keyCode == KeyCode.F)
        {
            if (isOpened)
            {
                GameData.SelectedStage = nextStage;
                StartCoroutine(UIManager.Instance.FadeScene(0, 1, "GameScene"));
            }
        }
    }
    public void OnDisable()
    {
        GameManager.Instance.OnClear -= ClearPortal;
    }
    public void OpenPortal()
    {
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = openSprite;
        isOpened = true;
        animator.SetBool("IsOpened", true);
    }
    public void ClosePortal()
    {
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = closedSprite;
        isOpened = false;
    }
    public void ClearPortal()
    {
        OpenPortal();
        promptContent = "[F] 다음 스테이지로 이동";
    }
    public bool CanInteract { get; } = true;
    public KeyCode[] KeyCodes { get; private set; } = { KeyCode.F };
}
