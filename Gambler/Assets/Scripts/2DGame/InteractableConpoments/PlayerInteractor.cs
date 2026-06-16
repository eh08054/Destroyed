using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerInteractor : MonoBehaviour
{
    public event Action<IInteractable, GameObject> OnHoverEntered;
    public event Action<IInteractable, GameObject> OnHoverExited;
    public event Action<IInteractable, GameObject> OnInteracted;

    private IInteractable CurrentInteractable;
    private GameObject CurrentInteractableObject;
    public LayerMask interactionLayerMask;
    public float interactionRange = 1;

    private void Update()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, interactionRange, interactionLayerMask);

        IEnumerable<Collider2D> orderedColliders = colliders.OrderBy(c => Vector2.Distance(transform.position, c.transform.position));

        bool foundInteractable = false;
        bool stayingOnCurrent = false;

        foreach(var c in orderedColliders)
        {
            var interactable = c.GetComponentInParent<IInteractable>(true);
            var interactableObject = c.gameObject;
            if(interactable != null && interactable.CanInteract)
            {
                if (CurrentInteractable != null)
                {
                    foreach (var x in CurrentInteractable.KeyCodes)
                    {
                        if (Input.GetKeyDown(x))
                        {
                            CurrentInteractable.OnInteract(x);
                            OnInteracted.Invoke(CurrentInteractable, c.gameObject);
                        }
                    }
                }
                if (interactable == CurrentInteractable)
                {
                    stayingOnCurrent = true;
                    break;
                }
                foundInteractable = true;
                if(CurrentInteractable != null)
                {
                    CurrentInteractable.OnInteractionExited();
                    OnHoverExited?.Invoke(CurrentInteractable, c.gameObject);
                }
                CurrentInteractable = interactable;
                CurrentInteractableObject = interactableObject;
                CurrentInteractable.OnInteractionEntered();
                OnHoverEntered?.Invoke(CurrentInteractable, c.gameObject);
                break;
            }
        }

        if(!foundInteractable && !stayingOnCurrent && CurrentInteractable != null)
        {
            CurrentInteractable.OnInteractionExited();
            OnHoverExited?.Invoke(CurrentInteractable, CurrentInteractableObject);
            CurrentInteractable = null;
        }
    }
}
