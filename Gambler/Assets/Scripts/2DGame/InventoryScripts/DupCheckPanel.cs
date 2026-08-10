using UnityEngine;
using UnityEngine.UI;
using System;

public class DupCheckPanel : MonoBehaviour
{
    [SerializeField] private Button yesButton;
    [SerializeField] private Button noButton;

    private Action onYes;
    private Action onNo;
    private void Awake()
    {
        yesButton.onClick.AddListener(YesClicked);
        noButton.onClick.AddListener(NoClicked);
        gameObject.SetActive(false);
    }

    public void Show(Action onYes, Action onNo)
    {
        this.onYes = onYes;
        this.onNo = onNo;
        gameObject.SetActive(true);
    }
    private void YesClicked()
    {
        gameObject.SetActive(false);
        var callback = onYes;
        onYes = null;
        onNo = null;
        callback.Invoke();
    }
    private void NoClicked()
    {
        gameObject.SetActive(false);
        var callback = onNo;
        onYes = null;
        onNo = null;
        callback.Invoke();
    }
}
