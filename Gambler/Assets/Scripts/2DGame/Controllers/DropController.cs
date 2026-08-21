using UnityEngine;
using UnityEngine.EventSystems;
public class DropController : MonoBehaviour, IDropHandler
{
    private SkillController skillController;
    [SerializeField] private int index;
    void Start()
    {
        skillController = GameObject.FindGameObjectWithTag("Player").GetComponent<SkillController>();
    }
    void IDropHandler.OnDrop(PointerEventData eventData)
    {
        DragController draggedSkill = eventData.pointerDrag.GetComponent<DragController>();
        if(draggedSkill != null)
        {
            Skill skill = draggedSkill.Skill;
            skillController.EquipSkillFromDrag(index, skill);
        }
    }
}
