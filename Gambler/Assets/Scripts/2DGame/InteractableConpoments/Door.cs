using System;
using UnityEngine;

public class Door : MonoBehaviour, IInteractable
{
    [SerializeField] private Sprite openSprite;
    [SerializeField] private Sprite closedSprite;
    [SerializeField] private int nextStage;
    [SerializeField] private string promptContent;
    public string PromptContent => promptContent;
    private bool isOpened = false;
    private bool isClicked = false;
    public void OnEnable()
    {
        GameManager.Instance.OnClear += ClearDoor;
    }
    public void OnInteractionEntered()
    {

    }
    public void OnInteractionExited()
    {

    }
    public void OnInteract(KeyCode keyCode)
    {
        if (keyCode == KeyCode.E)
        {
            if (!isOpened)
            {
                OpenDoor();
                promptContent = "[E] 문 닫기" + "\n" + "[F] 나가기";

            }
            else
            {
                CloseDoor();
                promptContent = "[E] 문 열기";
            }
        }
        else if(keyCode == KeyCode.F)
        {
            if (isOpened && !isClicked)
            {
                GameData.SelectedStage = nextStage;
                StartCoroutine(UIManager.Instance.FadeScene(0, 1, "GameScene"));
                isClicked = true;
            }
        }
    }
    public void OnDisable()
    {
        GameManager.Instance.OnClear -= ClearDoor;
    }
    public void OpenDoor()
    {
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = openSprite;
        isOpened = true;
    }
    public void CloseDoor()
    {
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = closedSprite;
        isOpened = false;
    }
    public void ClearDoor()
    {
        OpenDoor();
        promptContent = "[F] 다음 스테이지로 이동";
        KeyCodes = new KeyCode[] { KeyCode.F };
    }
    public bool CanInteract { get; } = true;
    public KeyCode[] KeyCodes { get; private set; } = { KeyCode.E, KeyCode.F };
}
