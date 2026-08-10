using UnityEngine;
using System.Collections.Generic;
public class ActiveItemPanel : MonoBehaviour
{
    public Dictionary<ItemData, BuffSlot> ActiveBuffSlots { get; set; }

    private void Awake()
    {
        ActiveBuffSlots = new Dictionary<ItemData, BuffSlot>();
    }
}
