using UnityEngine;
using TMPro;
public class InteractionPromptView : MonoBehaviour
{
    [SerializeField] private TMP_Text promptText;
    public bool IsVisible { get; private set; }
    public void ShowPrompt(string text)
    {
        IsVisible = true;
        promptText.enabled = true;
        promptText.text = text;
    }
    public void HidePrompt()
    {
        IsVisible = false;
        promptText.enabled = false;
    }
}
