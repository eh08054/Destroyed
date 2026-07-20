using UnityEngine;
using System.Collections.Generic;
using UnityEngine.InputSystem;

public class TreasureBox : MonoBehaviour, IInteractable
{
    [SerializeField] private string promptContent;
    [SerializeField] private TreasureData treasureData;
    public string PromptContent => promptContent;
    private Animator _animator;
    private bool isOpened = false;
    public void Start()
    {
        _animator = GetComponent<Animator>();
        _animator.SetBool("Idle", true);
        promptContent = "[E] 상자 열기";
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
                OpenTreasure();
                promptContent = "";
            }
        }
    }
    public void OpenTreasure()
    {
        isOpened = true;
        KeyCodes = new KeyCode[] {};
        SetState(open: true);
        MyEffectManager.Instance.CreateSpriteEffect(gameObject, "Fall");
        GetRewards();
    }
    private void GetRewards()
    {
        foreach(var item in treasureData.items)
        {
            GameObject x = Instantiate(item._itemPrefab, transform.position + Vector3.right, Quaternion.identity);
            if(item.itemType == ItemData.ItemType.Gold)
            {
                x.GetComponent<GoldDrop>().amount = treasureData.gold;
            }
        }
    }
    private void SetState(bool idle = false, bool destroy = false, bool open = false)
    {
        _animator.SetBool("Idle", idle);
        _animator.SetBool("Destroy", destroy);
        _animator.SetBool("Open", open);
    }
    public bool CanInteract { get; } = true;
    public KeyCode[] KeyCodes { get; private set; } = { KeyCode.E };
}
