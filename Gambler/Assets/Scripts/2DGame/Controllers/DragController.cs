using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
public class DragController : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler
{
    private Canvas canvas;
    private GameObject dragCopy;
    private RectTransform dragCopyRect;

    public Skill Skill { get; private set; }
    void IBeginDragHandler.OnBeginDrag(PointerEventData eventData)
    {
        canvas = GetComponentInParent<Canvas>();
        dragCopy = Instantiate(gameObject, canvas.transform); 
        dragCopyRect = dragCopy.GetComponent<RectTransform>();
        RectTransform originalRect = GetComponent<RectTransform>();

        dragCopyRect.sizeDelta = new Vector2(
            originalRect.rect.width * originalRect.lossyScale.x / canvas.transform.lossyScale.x,
            originalRect.rect.height * originalRect.lossyScale.y / canvas.transform.lossyScale.y);

        Vector2 screenPos = RectTransformUtility.WorldToScreenPoint(null, originalRect.position);
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            canvas.GetComponent<RectTransform>(),
            screenPos,
            null,
            out Vector2 localPos);
        dragCopyRect.anchoredPosition = localPos;
        dragCopy.GetComponent<Image>().raycastTarget = false;
        Destroy(dragCopy.GetComponent<DragController>());

        Skill = GetComponentInParent<SkillContainerSlot>().skill;
    }
    void IDragHandler.OnDrag(PointerEventData eventData)
    {
        if(dragCopy == null) { return; }
        dragCopyRect.anchoredPosition += eventData.delta / canvas.scaleFactor;
    }
    void IEndDragHandler.OnEndDrag(PointerEventData eventData)
    {
        if(dragCopy != null)
        {
            Destroy(dragCopy);
        }
    }
}
