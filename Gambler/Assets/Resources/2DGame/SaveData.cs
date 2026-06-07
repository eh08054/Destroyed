using UnityEngine;
using System.Collections.Generic;
[System.Serializable]
public class SaveData 
{
    public List<string> item_names;
    public int gold;
    public SaveData()
    {
        item_names = new List<string>();
        gold = 0;
    }
}
