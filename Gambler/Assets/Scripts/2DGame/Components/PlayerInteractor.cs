using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerInteractor : MonoBehaviour
{
    public event Action<IInteractable> OnHoverEntered;
    public event Action<IInteractable> OnHoverExited;
    public event Action<IInteractable> OnInteracted;

    private IInteractable CurrentInteractable;
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
            if(interactable != null && interactable.CanInteract)
            {
                if (CurrentInteractable != null)
                {
                    foreach (var x in CurrentInteractable.KeyCodes)
                    {
                        if (Input.GetKeyDown(x))
                        {
                            CurrentInteractable.OnInteract(x);
                            OnInteracted.Invoke(CurrentInteractable);
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
                    OnHoverExited?.Invoke(CurrentInteractable);
                }
                CurrentInteractable = interactable;
                CurrentInteractable.OnInteractionEntered();
                OnHoverEntered?.Invoke(CurrentInteractable);
                break;
            }
        }

        if(!foundInteractable && !stayingOnCurrent && CurrentInteractable != null)
        {
            CurrentInteractable.OnInteractionExited();
            OnHoverExited?.Invoke(CurrentInteractable);
            CurrentInteractable = null;
        }
    }
}
