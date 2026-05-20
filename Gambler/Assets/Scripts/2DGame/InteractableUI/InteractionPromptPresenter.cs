using UnityEngine;

public class InteractionPromptPresenter : MonoBehaviour
{
    [SerializeField] private InteractionPromptView view;
    private PlayerInteractionModel interactionModel;

    private void Start()
    {
        interactionModel = GameManager.Instance.Player.GetComponent<PlayerInteractionModel>();
        interactionModel.OnInteractableChanged += HandleInteractionView;
    }

    private void HandleInteractionView(bool isInteractable)
    {
        if (isInteractable)
        {
            view.ShowPrompt(interactionModel.PromptContent);
        }
        else
        {
            view.HidePrompt();
        }
    }
    private void OnDestroy()
    {
        interactionModel.OnInteractableChanged -= HandleInteractionView;
    }
}
