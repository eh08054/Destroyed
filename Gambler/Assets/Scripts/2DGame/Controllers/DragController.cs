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
        dragCopyRect.position = GetComponent<RectTransform>().position;

        dragCopy.GetComponent<Image>().raycastTarget = false;
        Destroy(dragCopy.GetComponent<DragController>());
        Skill = GetComponentInParent<SkillContainerSlot>().skill;
    }
    void IDragHandler.OnDrag(PointerEventData eventData)
    {
        Debug.Log("SDfsf");
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
