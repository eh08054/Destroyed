using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonAnimation : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private Animator characterAnimator;

    public void OnPointerEnter(PointerEventData eventData)
    {
        characterAnimator.SetTrigger("Run");   // 하이라이트 시 걷기
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        characterAnimator.SetTrigger("Idle");  // 해제 시 Idle
    }
}
