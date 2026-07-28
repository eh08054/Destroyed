using UnityEngine;
using System.Collections.Generic;
[System.Serializable]
public class SaveData 
{
    public List<string> item_names;
    public int gold;
    public float BGMVolume;
    public float SFXVolume;
    public float AllVolume;
    public float HUDAlpha;
    public SaveData()
    {
        item_names = new List<string>();
        gold = 0;
        BGMVolume = 0;
        SFXVolume = 0;
        AllVolume = 0;
        HUDAlpha = 0;
    }
}
