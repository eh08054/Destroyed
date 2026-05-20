using UnityEngine;

public class Door : MonoBehaviour, IInteractable
{
    [SerializeField] private Sprite openSprite;
    [SerializeField] private Sprite closedSprite;
    public string PromptContent { get; private set; } = "[E] 문 열기";
    private bool isOpened = false;
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
            SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
            if (!isOpened)
            {
                spriteRenderer.sprite = openSprite;
                isOpened = true;
                PromptContent = "[E] 문 닫기" + "\n" + "[F] 나가기";
            }
            else
            {
                spriteRenderer.sprite = closedSprite;
                isOpened = false;
                PromptContent = "[E] 문 열기";
            }
        }
        else if(keyCode == KeyCode.F)
        {
            if (isOpened)
            {
                GameManager.Instance.SceneChanger.SceneChange("GameScene");
            }
        }
    }
    public bool CanInteract { get; } = true;
    public KeyCode[] KeyCodes { get; } = { KeyCode.E, KeyCode.F };
}
