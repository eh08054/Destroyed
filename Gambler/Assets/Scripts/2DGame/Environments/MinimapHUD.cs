using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using TMPro;
public class MinimapHUD : MonoBehaviour
{
    [System.Serializable]
    public class EnemyCountEntry
    {
        public Image icon;
        public TMP_Text countText;
        public int enemyCount;
    }
    [SerializeField] private List<EnemyCountEntry> enemyCountEntries;
    private Dictionary<string, EnemyCountEntry> enemyDict;

    public void InitHUD(List<EnemySpawnInfo> enemySpwanInfos)
    {
        int index = 0;
        enemyDict = new Dictionary<string, EnemyCountEntry>();
        foreach(var enemySpawnInfo in enemySpwanInfos)
        {
            if(index >= enemyCountEntries.Count) { return; }
            enemyCountEntries[index].icon.sprite = enemySpawnInfo.enemyData.enemyIcon;
            enemyCountEntries[index].icon.color = new Color(1, 1, 1, 1);
            enemyCountEntries[index].enemyCount = enemySpawnInfo.count;
            enemyCountEntries[index].countText.text = enemyCountEntries[index].enemyCount.ToString();
            enemyDict.Add(enemySpawnInfo.enemyData.enemyName, enemyCountEntries[index]);
            index++;
        }
        while(index < enemyCountEntries.Count)
        {
            enemyCountEntries[index].icon.color = new Color(1, 1, 1, 0);
            enemyCountEntries[index].countText.text = "";
            index++;
        }
    }
    public void RefreshHUD(string enemyName)
    {
        enemyDict[enemyName].enemyCount--;
        enemyDict[enemyName].countText.text = enemyDict[enemyName].enemyCount.ToString();
    }
}
